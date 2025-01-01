using System.Net;
using Microsoft.AspNetCore.Http;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Infrastructure.Models;

namespace SertifierCase.Infrastructure.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Response responseError;
            var response = context.Response;

            switch (exception)
            {
                case SertifierException e:
                    responseError = e.Response;
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                default:
                    responseError = CustomErrors.E_100;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await response.WriteAsJsonAsync(responseError);
        }
    }
}