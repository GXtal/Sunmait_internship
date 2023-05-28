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
        catch (BrandInUseException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Brand is in use",
                Detail = ex.Message,
            };

            await context.Response.WriteAsJsonAsync(details);
        }
        catch (ExistingBrandNameException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Brand with this name already exists",
                Detail = ex.Message,
            };

            await context.Response.WriteAsJsonAsync(details);
        }
        catch (BrandNotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            ProblemDetails details = new ProblemDetails()
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Brand not found",
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
