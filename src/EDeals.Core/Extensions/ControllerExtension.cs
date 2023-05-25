using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EDeals.Core.API.Extensions
{
    public static class ControllerExtension
    {
        public static ActionResult Map(ResultResponse result)
        {
            var responseObject = new ResponseObject(Errors: result.Errors);

            return result.Result switch
            {
                ResultCode.Ok => new OkObjectResult(responseObject),
                ResultCode.BadRequest => new BadRequestObjectResult(responseObject),
                ResultCode.Unauthorized => new UnauthorizedObjectResult(responseObject),
                ResultCode.Forbidden => new ObjectResult(responseObject) { StatusCode = (int)HttpStatusCode.Forbidden },
                ResultCode.NotFound => new NotFoundObjectResult(responseObject),
                _ => new ObjectResult(responseObject) { StatusCode = (int)HttpStatusCode.InternalServerError },
            };
        }

        public static ActionResult<T> Map<T>(ResultResponse<T> result)
        {
            var responseObject = new ResponseObject(result.ResponseData, result.Errors);

            return result.Result switch
            {
                ResultCode.Ok => new OkObjectResult(responseObject),
                ResultCode.BadRequest => new BadRequestObjectResult(responseObject),
                ResultCode.Unauthorized => new UnauthorizedObjectResult(responseObject),
                ResultCode.Forbidden => new ObjectResult(responseObject) { StatusCode = (int)HttpStatusCode.Forbidden },
                ResultCode.NotFound => new NotFoundObjectResult(responseObject),
                _ => new ObjectResult(responseObject) { StatusCode = (int)HttpStatusCode.InternalServerError },
            };
        }

        public record ResponseObject(object? ResponseData = null, List<ResponseError>? Errors = null);
    }
}
