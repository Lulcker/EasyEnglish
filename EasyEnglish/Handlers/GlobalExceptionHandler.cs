using System.Net.Mime;
using EasyEnglish.Application;
using EasyEnglish.DTO;
using Microsoft.AspNetCore.Diagnostics;

namespace EasyEnglish.Handlers;

internal class GlobalExceptionHandler
{
    internal static async Task Handle(HttpContext context)
    {
        var exception =
            context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
        
        var logger = context.RequestServices.GetService<ILogger<GlobalExceptionHandler>>()!;

        switch (exception)
        {
            // доступ запрещён
            case AccessDeniedException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.WritePlainToResponse(exception.Message);
                break;
            // бизнес-исключение
            case BusinessException:
                logger.LogInformation("Business exception: {Message}", exception.Message);
                
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.WritePlainToResponse(exception.Message);
                break;
            case ConfirmActionException:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.WritePlainToResponse(exception.Message);
                break;
            // неожиданное исключение
            default:
                logger.LogError(exception, "Произошла необработанная ошибка: {Error}", exception.Message);
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.WritePlainToResponse("Произошла внутренняя ошибка системы");
                break;
        }
    }
}

internal static class GlobalExceptionHandlerExtensions
{
    internal static async Task WritePlainToResponse(this HttpContext context, string message)
    {
        context.Response.ContentType = $"{MediaTypeNames.Application.Json}; charset=utf-8";

        await context.Response.WriteAsJsonAsync(new ErrorResponseModel { Error = message });
    }
}