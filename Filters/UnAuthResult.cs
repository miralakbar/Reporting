using Application.CQRS.Core;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Application.Extensions;

namespace API.Filters
{
    public class UnAuthResult
    {
        public int? Output { get; set; }
        public Error Error { get; set; } = new Application.CQRS.Core.Error();
        public bool IsSuccess => Error.HttpStatus == HttpStatusCode.OK;
    }

    public class UnAuthActionResult : IActionResult
    {
        private readonly string _errorMessage;

        public UnAuthActionResult() { }

        public UnAuthActionResult(string message)
        {
            _errorMessage = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = new ApiResult<object>
            {
                Error = new Error
                {
                    HttpStatus = HttpStatusCode.Unauthorized,
                    ErrorCode = CustomExceptionCodes.UnAuthorizedException,
                    ErrorMessage = string.IsNullOrEmpty(_errorMessage) ? CustomExceptionCodes.UnAuthorizedException.GetEnumDescription() : _errorMessage
                }
            };

            context.HttpContext.Response.StatusCode = (int)response.Error.HttpStatus;
            await new ObjectResult(response).ExecuteResultAsync(context);
        }
    }
}