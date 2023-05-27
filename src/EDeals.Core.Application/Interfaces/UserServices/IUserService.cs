using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.User;

namespace EDeals.Core.Application.Interfaces.UserServices
{
    public interface IUserService
    {
        Task<ResultResponse<UserInfoResponse>> GetUserInfo();
        Task<ResultResponse> DeleteUserAsync();
    }
}
