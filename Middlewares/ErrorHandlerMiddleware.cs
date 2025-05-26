using Application.CQRS.Core;
using Application.CustomExceptions;
using Application.CustomExceptions.SharedExceptions;
using Application.Extensions;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Middlewares
{
    // todo clone connection
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private async Task HandleError(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;


            var result = ApiResult<string>.ERROR(new Error
            {
                ErrorCode = CustomExceptionCodes.UnHandledException,
                HttpStatus = System.Net.HttpStatusCode.InternalServerError,
                ErrorMessage = CustomExceptionCodes.UnHandledException.GetEnumDescription()
            });

            var innerExceptionMessage = exception.InnerException?.Message;
            var innerStackTrace = exception.InnerException?.StackTrace;

            string errorMessage = $"Custom Exception: \n: {CustomExceptionCodes.UnHandledException} \n" +
                              "Original exception: \n" + exception?.Message + "\n" +
                              "Inner exception: \n" + innerExceptionMessage + "\n" +
                              "Stack Trace: \n" + innerStackTrace + "\n";

            _logger.LogInformation(errorMessage);
            SentrySdk.CaptureException(new UnHandledException(errorMessage));

            await response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}
