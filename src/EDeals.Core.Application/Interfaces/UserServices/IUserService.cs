using EDeals.Core.Application.Models;
using EDeals.Core.Application.Models.UserProfile;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;

namespace EDeals.Core.Application.Interfaces.UserServices
{
    public interface IUserService
    {
        Task<ResultResponse<UserInfoResponse>> GetUserInfo();
        Task<ResultResponse> DeleteUserAsync();
        Task<ResultResponse<List<UserInfoResponse>>> GetUsers(string username);
        Task<ResultResponse> UpdateUser(UpdateUserModel model, string token);
        Task<ResultResponse> BlockUser(Guid userId);
        Task<ResultResponse> UnBlockUser(Guid userId);
        Task<ResultResponse<List<UserInfoResponse>>> GetUsersAdmin();
    }
}
