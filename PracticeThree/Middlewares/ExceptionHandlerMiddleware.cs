using System.Globalization;
using Serilog;

namespace UPB.PracticeThree.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
            HandleException(context, ex);
        }

    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "text/json";
        return context.Response.WriteAsync("Sucedio el sgte error: " + ex.Message);
    }
}

public static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}