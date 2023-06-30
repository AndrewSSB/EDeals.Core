using EDeals.Core.Application.Models.UserProfile;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Interfaces.UserServices
{
    public interface IUserService
    {
        Task<ResultResponse<UserInfoResponse>> GetUserInfo();
        Task<ResultResponse> DeleteUserAsync();
        Task<ResultResponse<List<UserInfoResponse>>> GetUsers();
    }
}
