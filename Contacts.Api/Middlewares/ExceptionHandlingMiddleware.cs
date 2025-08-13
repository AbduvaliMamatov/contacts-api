using System.Net;
using Contacts.Api.Exceptions;

namespace Contacts.Api.Middlewares;

public class ExceptionHandlingMiddleware(
    ILogger<ExceptionHandlingMiddleware> logger,
    RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Anhandled exception occured");

            (int code, string message) = ex switch
            {
                CustomBadRequestException badRequest => ((int)HttpStatusCode.BadRequest, badRequest.Message),
                CustomConflictException conflict => ((int)HttpStatusCode.Conflict, conflict.Message),
                CustomNotFoundException notFound => ((int)HttpStatusCode.NotFound, notFound.Message),
                _ => ((int)HttpStatusCode.InternalServerError, ex.Message)
            };

            context.Response.StatusCode = code;
            await context.Response.WriteAsync(message);
        }
    }
}