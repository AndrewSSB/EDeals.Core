using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.ErrorMessages.Auth;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Enums;
using EDeals.Core.Domain.Models.Authentiation.Login;
using EDeals.Core.Domain.Models.Authentiation.Register;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public class IdentityRepository : Result, IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<IdentityRepository> _logger;

        public IdentityRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            ILogger<IdentityRepository> logger)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<ResultResponse<RegisterResponse>> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password)
        {
            var user = new ApplicationUser(firstName, lastName, userName, email, phoneNumber);

            var userCreationResult = await _userManager.CreateAsync(user, password);

            if (!userCreationResult.Succeeded)
            {
                _logger.LogError("Account creation failed - ", userCreationResult.Errors);
                return BadRequest<RegisterResponse>(new ResponseError(ErrorCodes.AccountCreation, ResponseErrorSeverity.Error, RegisterMessages.CreationFailure));
            }
            
            var assigningUserRoleResult = await _userManager.AddToRoleAsync(user, RoleNames.UserRole);

            if (!assigningUserRoleResult.Succeeded)
            {
                _logger.LogError("Account role asigning failed - ", assigningUserRoleResult.Errors);
                return BadRequest<RegisterResponse>(new ResponseError(ErrorCodes.AssigningRole, ResponseErrorSeverity.Error, RegisterMessages.RoleAssigningFailure));
            }

            // TODO: Add token generation logic here

            return Ok<RegisterResponse>();
        }

        public async Task<ResultResponse<LoginResponse>> SignInUserAsync(string password, Guid? userId = null, string? email = null, string? username = null)
        {
            var user = await GetUser(userId, email, username);

            if (user is null)
            {
                _logger.LogError("User with id {userId} / email {email} / username {username} does not exist", userId, email, username);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InvalidUsernameOrPassword, ResponseErrorSeverity.Error, LoginMessages.InvalidUsernameOrPassword));
            }

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (!checkPasswordResult.Succeeded)
            {
                _logger.LogError("Incorrect password");
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InvalidUsernameOrPassword, ResponseErrorSeverity.Error, LoginMessages.InvalidUsernameOrPassword));
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                _logger.LogError("Can't sign in");
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InternalServer, ResponseErrorSeverity.Error, GenericMessages.InternalError));
            }

            var signInConfiguration = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1),
            };

            await _signInManager.SignInAsync(user, signInConfiguration);

            // TODO: Add token generation logic here

            return Ok<LoginResponse>();
        }

        public async Task<ResultResponse> SignOutUserAsync(Guid userId)
        {
            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InternalServer, ResponseErrorSeverity.Error, GenericMessages.GenericMessage));
            }

            await _signInManager.SignOutAsync();

            return Ok();
        }

        public async Task<ResultResponse> DeleteUserAsync(Guid? userId, string? email, string? username)
        {
            var user = await GetUser(userId, email, username);

            return user != null ? await DeleteUserAsync(user) : Ok();
        }

        public async Task<ApplicationUser?> FindUser(Guid? userId, string? email, string? username) =>
            await GetUser(userId, email, username);

        public async Task<ResultResponse> GetUserNameAsync(Guid userId) =>
            Ok(await _userManager.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefaultAsync());

        public async Task<ResultResponse> GetUserEmailAsync(Guid userId) =>
            Ok(await _userManager.Users.Where(x => x.Id == userId).Select(x => x.Email).FirstOrDefaultAsync());

        public async Task<ResultResponse> GetUserPhoneNumberAsync(Guid userId) =>
            Ok(await _userManager.Users.Where(x => x.Id == userId).Select(x => x.PhoneNumber).FirstOrDefaultAsync());

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync();

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        #region Private methods
        private async Task<ResultResponse> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        private async Task<ApplicationUser?> GetUser(Guid? userId = null, string? email = null, string? username = null)
        {
            var user = new ApplicationUser();

            if (userId is not null)
            {
                user = await _userManager.FindByIdAsync(userId.ToString()!);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                user = await _userManager.FindByEmailAsync(email);
            }
            else if (!string.IsNullOrEmpty(username))
            {
                user = await _userManager.FindByNameAsync(username);
            }

            return user;
        }
        #endregion
    }
}
