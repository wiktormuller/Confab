using System.Collections.Concurrent;
using System.Net;
using Confab.Shared.Abstractions.Exceptions;
using Humanizer;

namespace Confab.Shared.Infrastructure.Exceptions;

internal class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();
    
    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            ConfabException ex => new ExceptionResponse(new ErrorResponse(new Error(GetErrorCode(ex), ex.Message)), 
                HttpStatusCode.BadRequest),
            _ => new ExceptionResponse(new ErrorResponse(new Error("error", "There was an error.")),
                HttpStatusCode.InternalServerError)
        };

    private record Error(string Code, string Message);

    private record ErrorResponse(params Error[] Errors);

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        var code = type.Name.Underscore().Replace("_exception", string.Empty);
        
        return Codes.GetOrAdd(type, code);
    }
}