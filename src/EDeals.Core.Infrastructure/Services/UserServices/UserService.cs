using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.User;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using EDeals.Core.Infrastructure.Identity.Repository;
using EDeals.Core.Infrastructure.Shared.ExecutionContext;
using Microsoft.AspNetCore.Identity;

namespace EDeals.Core.Infrastructure.Services.UserServices
{
    public class UserService : Result, IUserService
    {
        private readonly IIdentityRepository _identityRepo;
        private readonly ICustomExecutionContext _executionContext; 
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ICustomExecutionContext executionContext, IIdentityRepository identityRepo, UserManager<ApplicationUser> userManager)
        {
            _executionContext = executionContext;
            _identityRepo = identityRepo;
            _userManager = userManager;
        }

        public async Task<ResultResponse<UserInfoResponse>> GetUserInfo()
        {
            var userId = _executionContext.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return Ok(new UserInfoResponse());
        }

        public async Task<ResultResponse> DeleteUserAsync()
        {
            // TODO: maybe signout the user, and move to soft delete
            var userId = _executionContext.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user != null ? await DeleteUserAsync(user) : Ok();
        }

        private async Task<ResultResponse> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
