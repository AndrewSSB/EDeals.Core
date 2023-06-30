using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Application.Models.UserProfile;
using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using EDeals.Core.Infrastructure.Identity.Repository;
using EDeals.Core.Infrastructure.Shared.ExecutionContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EDeals.Core.Infrastructure.Services.UserServices
{
    public class UserService : Result, IUserService
    {
        private readonly ICustomExecutionContext _executionContext; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public UserService(ICustomExecutionContext executionContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _executionContext = executionContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<ResultResponse<UserInfoResponse>> GetUserInfo()
        {
            var userId = _executionContext.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return BadRequest<UserInfoResponse>(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            return Ok(new UserInfoResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName, 
                LastName = user.LastName,
                IsEmailVerified = user.EmailConfirmed,
                IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
            });
        }

        public async Task<ResultResponse> DeleteUserAsync()
        {
            var userId = _executionContext.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return BadRequest(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            user.UserName += $"_{DateTime.UtcNow}";
            user.IsDeleted = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
            }

            return result.ToApplicationResult();
        }

        public async Task<ResultResponse<List<UserInfoResponse>>> GetUsers()
        {
            return Ok(await _context.Users.Select(user => new UserInfoResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsEmailVerified = user.EmailConfirmed,
                IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
            })
            .ToListAsync());
        }
    }
}
