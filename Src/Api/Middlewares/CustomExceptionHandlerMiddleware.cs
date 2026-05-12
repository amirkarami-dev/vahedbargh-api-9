using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Sentry;
using Stripe;
using AuthenticationException = Coreapi.Application.Common.Exceptions.AuthenticationException;

namespace Coreapi.Api.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.UnsupportedMediaType;
                    result = JsonConvert.SerializeObject(validationException.Failures);
                    break;
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    break;
                case StripeException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = badRequestException.StripeError.Message;
                    break;
                case AuthenticationException authenticationException:
                    code = HttpStatusCode.BadRequest;
                    result = string.Join(',', authenticationException.Errors);
                    break;
                case System.Security.Authentication.AuthenticationException authenticationException:
                    code = HttpStatusCode.BadRequest;
                    result = string.Format("{0} -> {1}", authenticationException.Message, authenticationException.InnerException?.Message);
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = notFoundException.Message;
                    break;
                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    result = "Invalid Token";
                    break;
                default:
                    SentrySdk.CaptureException(exception);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)
            {
                result = string.Format("{0} -> {1}", exception.Message, exception.InnerException?.Message);
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
