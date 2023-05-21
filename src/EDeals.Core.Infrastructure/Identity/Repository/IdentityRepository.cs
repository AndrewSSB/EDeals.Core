using EDeals.Core.Domain.Enums;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;

        public IdentityRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
        }

        public async Task<string?> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password)
        {
            var user = new ApplicationUser(firstName, lastName, userName, email, phoneNumber);

            var userCreationResult = await _userManager.CreateAsync(user, password);

            var assigningUserRoleResult = await _userManager.AddToRoleAsync(user, RoleNames.UserRole);

            if (userCreationResult is null)
            {
                //...
            }

            if (assigningUserRoleResult is null)
            {
                //...
            }

            // TODO: Add token generation logic here

            return null;
        }

        public async Task<string?> SignInUserAsync(Guid? userId, string? email, string? username, string password)
        {
            var user = await GetUser(userId, email, username);

            if (user is null)
            {
                return "Failed";
            }

            var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (!checkPasswordResult.Succeeded)
            {
                return "Failed";
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                return "Failed";
            }

            var signInConfiguration = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1),
            };

            await _signInManager.SignInAsync(user, signInConfiguration);

            // TODO: Add token generation logic here

            return "Success";
        }

        public async Task<string?> SignOutUserAsync(Guid userId)
        {
            var user = await GetUser(userId);

            if (user is null)
            {
                return "Failed";
            }

            await _signInManager.SignOutAsync();

            return "Logged out";
        }

        public async Task<string?> DeleteUserAsync(Guid? userId, string? email, string? username)
        {
            var user = await GetUser(userId, email, username);

            return user != null ? await DeleteUserAsync(user) : "Success";
        }

        public async Task<ApplicationUser?> FindUser(Guid? userId, string? email, string? username) =>
            await GetUser(userId, email, username);

        public async Task<string?> GetUserNameAsync(Guid userId) =>
            await _userManager.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefaultAsync();

        public async Task<string?> GetUserEmailAsync(Guid userId) =>
            await _userManager.Users.Where(x => x.Id == userId).Select(x => x.Email).FirstOrDefaultAsync();

        public async Task<string?> GetUserPhoneNumberAsync(Guid userId) =>
            await _userManager.Users.Where(x => x.Id == userId).Select(x => x.PhoneNumber).FirstOrDefaultAsync();

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync();

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        #region Private methods
        private async Task<string> DeleteUserAsync(ApplicationUser user)
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
