using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace EasyEnglish.Generators;

[Generator]
public class ApiEndpointHelperGenerator : ISourceGenerator
{
    private const string TargetAssembly = "EasyEnglish";

    private const string ClassTemplate = """
                                         // generated
                                         #nullable enable
                                         
                                         using System.Net.Http;
                                         using [Namespace];
                                         using EasyEnglish.DTO.[UsingPrefixController]s.RequestModels;
                                         using EasyEnglish.DTO.[UsingPrefixController]s.ResponseModels;
                                         
                                         namespace [Namespace].ApiMethods;
                                         
                                         public class [ClassName](IHttpHelper httpHelper)
                                         {
                                             [Methods]
                                         }
                                         """;
    
    private const string DependenciesClassTemplate = """
                                                     // generated
                                                     using [Namespace].ApiMethods;
                                                     using Microsoft.Extensions.DependencyInjection;

                                                     namespace [Namespace];

                                                     public static class ApiHelpersConfigurator
                                                     {
                                                         public static IServiceCollection AddApiHelpers(this IServiceCollection services)
                                                         {
                                                             [Dependencies]
                                                             
                                                             return services;
                                                         }
                                                     }
                                                     """;

    private const string MethodTemplate = "public async Task[ReturnType] [MethodName]([Parameters] = default) =>\n\t\t[CallHttpHelper]";

    private const string CallHttpHelper = "await httpHelper.SendAsync[TRequest|TResponse]([Path], [HttpMethod][RequestModel] cancellationToken);";

    private const string DependencyService = "services.AddScoped<[ServiceName]>();";
    
    public void Initialize(GeneratorInitializationContext context)
    {
        // Не нужно
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var assemblyName = context.Compilation.AssemblyName!;

        List<string> dependencyServices = [];
        
        foreach (var controller in GetControllersFromNamespace(GetProjectRootNamespace(context.Compilation.GlobalNamespace)))
        {
            context.AddSource(
                hintName: $"{GetClassName(controller)}.g.cs",
                sourceText: SourceText.From(GenerateControllerApiEndpointClass(controller, assemblyName), Encoding.UTF8)
                );
            
            dependencyServices.Add(DependencyService.Replace("[ServiceName]", GetClassName(controller)));
        }
        
        context.AddSource(
            hintName: "ApiHelpersConfigurator.g.cs",
            sourceText: SourceText.From(GenerateDependenciesClass(dependencyServices, assemblyName), Encoding.UTF8)
            );
    }
    
    private static INamespaceSymbol GetProjectRootNamespace(INamespaceSymbol globalNamespace)
    {
        var projectParts = TargetAssembly.Split('.');

        return projectParts
            .Aggregate(globalNamespace, (current, projectPart) =>
                current.GetNamespaceMembers().Single(n => n.Name.Contains(projectPart)));
    }
    
    private static List<INamedTypeSymbol> GetControllersFromNamespace(INamespaceSymbol @namespace)
    {
        var result = @namespace.GetTypeMembers()
            .Where(t => t.Name.EndsWith("Controller"))
            .ToList();

        var childrenNamespaces = @namespace.GetNamespaceMembers();
        foreach (var childNamespace in childrenNamespaces)
            result.AddRange(GetControllersFromNamespace(childNamespace));

        return result;
    }

    private static string GenerateControllerApiEndpointClass(INamedTypeSymbol controller, string assemblyName)
    {
        return ClassTemplate
            .Replace("[UsingPrefixController]", GetPrefixController(controller))
            .Replace("[Namespace]", assemblyName)
            .Replace("[ClassName]", GetClassName(controller))
            .Replace("[Methods]", GetMethods(controller));
    }

    private static string GenerateDependenciesClass(IReadOnlyCollection<string> dependencies, string assemblyName)
    {
        return DependenciesClassTemplate
            .Replace("[Namespace]", assemblyName)
            .Replace("[Dependencies]", string.Join("\n\t\t", dependencies));
    }

    private static string GetMethods(INamedTypeSymbol controller)
    {
        var basePath = controller
            .GetAttributes()
            .First(a => a.AttributeClass?.Name == "RouteAttribute")
            .ConstructorArguments[0].Value!.ToString();
        
        var methodsFromController = controller
            .GetMembers()
            .OfType<IMethodSymbol>()
            .Where(m => m.MethodKind == MethodKind.Ordinary)
            .ToList();

        if (methodsFromController.Count == 0)
            return string.Empty;

        List<string> methods = [];

        foreach (var method in methodsFromController)
        {
            var parameters = method.Parameters;

            var returnType = GetReturnType(method);
            
            var httpMethodPath = GetHttpAttribute(method)
                .ConstructorArguments[0].Value!.ToString();
            
            var requestModel = parameters
                .FirstOrDefault(c => c.GetAttributes()
                    .Any(a => a.AttributeClass!.Name == "FromBodyAttribute"));

            var callHttpHelper = CallHttpHelper
                .Replace("[TRequest|TResponse]", GetRequestResponseGenericType(requestModel, returnType))
                .Replace("[HttpMethod]", GetHttpMethod(method))
                .Replace("[RequestModel]", requestModel is null ? "," : $", {requestModel.Name},")
                .Replace("[Path]", GetFullPath(basePath, httpMethodPath, parameters));
            
            methods.Add(MethodTemplate
                .Replace("[ReturnType]", returnType == string.Empty ? string.Empty :  $"<{returnType}>")
                .Replace("[MethodName]", method.Name)
                .Replace("[Parameters]", string.Join(", ", parameters.Select(p => $"{p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {p.Name}")))
                .Replace("[CallHttpHelper]", callHttpHelper)
            );
        }

        return string.Join("\n\n\t", methods);
    }

    private static AttributeData GetHttpAttribute(IMethodSymbol method) =>
        method.GetAttributes()
            .First(a => a.AttributeClass!.Name.StartsWith("Http"));

    private static string GetHttpMethod(IMethodSymbol method)
    {
        var httpAttributeMethodName = GetHttpAttribute(method).AttributeClass!.Name;

        return httpAttributeMethodName switch
        {
            "HttpGetAttribute" => "HttpMethod.Get",
            "HttpPostAttribute" => "HttpMethod.Post",
            "HttpPatchAttribute" => "HttpMethod.Patch",
            "HttpPutAttribute" => "HttpMethod.Put",
            "HttpDeleteAttribute" => "HttpMethod.Delete",
            _ => string.Empty
        };
    }

    private static string GetRequestResponseGenericType(IParameterSymbol? requestModel, string returnType)
    {
        if (requestModel is not null && returnType != string.Empty)
            return $"<{requestModel.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}, {returnType}>";
        
        if (requestModel is not null && returnType == string.Empty)
            return $"<{requestModel.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>";
        
        if (requestModel is null && returnType != string.Empty)
            return $"<{returnType}>";

        return string.Empty;
    }

    private static string GetReturnType(IMethodSymbol method)
    {
        var returnType = method.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            
        if (returnType == "Task<IActionResult>")
            return string.Empty;

        returnType = returnType
            .Replace("Task<ActionResult<", string.Empty);

        return returnType.Substring(0, returnType.Length - 2);
    }
    
    private static string GetFullPath(string basePath, string methodPath, ImmutableArray<IParameterSymbol> parameters)
    {
        var queryParameters = parameters
            .Where(p => p.GetAttributes()
                .Any(a => a.AttributeClass!.Name == "FromQueryAttribute"))
            .Select(p => p.Name)
            .ToList();

        var queryParametersToString = string.Empty;
        if (queryParameters.Count > 0)
            queryParametersToString = $"?{string.Join("&", queryParameters.Select(q => $"{q}={{{q}}}"))}";

        return $"$\"{basePath}/{Regex.Replace(methodPath, @"\{([^:}]+):[^}]+\}", "{$1}")}{queryParametersToString}\"";
    }

    private static string GetPrefixController(INamedTypeSymbol controller) =>
        controller.Name.Substring(0, controller.Name.Length - 10);

    private static string GetClassName(INamedTypeSymbol controller) =>
        $"{GetPrefixController(controller)}ApiHelper";
}