using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Confab.Shared.Infrastructure.Exceptions;

internal class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IExceptionCompositionRoot _exceptionToResponseMapper;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger,
        IExceptionCompositionRoot exceptionToResponseMapper)
    {
        _logger = logger;
        _exceptionToResponseMapper = exceptionToResponseMapper;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorResponse = _exceptionToResponseMapper.Map(exception);

        context.Response.StatusCode = (int)(errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);
        
        var response = errorResponse?.Response;
        if (response is null)
        {
            return;
        }
        
        await context.Response.WriteAsJsonAsync(response);
    }
}