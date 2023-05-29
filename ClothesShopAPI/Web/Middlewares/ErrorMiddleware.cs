using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Middlewares;

public class ErrorMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Bad request",
                Detail = ex.Message,
            };

            await context.Response.WriteAsJsonAsync(details);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            ProblemDetails details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Not found",
                Detail = ex.Message,
            };

            await context.Response.WriteAsJsonAsync(details);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            throw ex;
        }
    }
}
