using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using Microsoft.AspNetCore.Identity;

namespace EDeals.Core.Infrastructure.Identity.Extensions
{
    public static class IdentityExtensions
    {
        public static ResultResponse ToApplicationResult(this IdentityResult result) =>
            result.Succeeded ?
                new ResultResponse()
                : 
                new ResultResponse(
                    ResultCode.InternalError, 
                    new ResponseError(ErrorCodes.InternalServer, ResponseErrorSeverity.Exception, result.Errors.Select(e => e.Description)?.FirstOrDefault()?.ToString() ?? GenericMessages.GenericMessage)
                );

    }
}
