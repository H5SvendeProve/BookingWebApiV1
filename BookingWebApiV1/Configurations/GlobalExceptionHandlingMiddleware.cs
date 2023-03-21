using System.Net;
using System.Text.Json;
using BookingWebApiV1.Exceptions;
using ArgumentException = BookingWebApiV1.Exceptions.ArgumentException;
using KeyNotFoundException = BookingWebApiV1.Exceptions.KeyNotFoundException;
using NotImplementedException = BookingWebApiV1.Exceptions.NotImplementedException;
using UnauthorizedAccessException = BookingWebApiV1.Exceptions.UnauthorizedAccessException;

namespace BookingWebApiV1.Configurations;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;
        var exceptionType = exception.GetType();

        if (exceptionType == typeof(NotImplementedException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotImplemented;
        }
        else if (exceptionType == typeof(BadRequestException))
        {
            message = exception.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(UnauthorizedAccessException))
        {
            message = exception.Message;
            status = HttpStatusCode.Unauthorized;
        }
        else if (exceptionType == typeof(KeyNotFoundException) || exceptionType == typeof(HeaderNotFoundException) || exceptionType == typeof(NotFoundException) || exceptionType == typeof(ArgumentException))
        {
            message = exception.Message;
            status = HttpStatusCode.NotFound;
        }
        
        else if (exceptionType == typeof(CustomSqlException) || exceptionType == typeof(CustomDbException) || exceptionType == typeof(ServerErrorException))
        {
            message = "error happened on database level. Contact administrator for further information";
            status = HttpStatusCode.InternalServerError;
        }
        else
        {
            message = "An error occurred server site.";
            status = HttpStatusCode.InternalServerError;
        }

        var response = new
        {
            error = message,
            status
        };

        var json = JsonSerializer.Serialize(response);
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}