using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace EasyEnglish.Configurators;

internal static class AutofacConfigurator
{
    internal static WebApplicationBuilder ConfigureAutofac(this WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterAssemblyModules(typeof(Program).Assembly));
        
        return builder;
    }
}