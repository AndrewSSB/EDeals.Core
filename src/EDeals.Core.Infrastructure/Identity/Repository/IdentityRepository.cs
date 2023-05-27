using EDeals.Core.Application.Interfaces.Email;
using EDeals.Core.Application.Interfaces.SMS;
using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.ErrorMessages.Auth;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Enums;
using EDeals.Core.Domain.Models.Authentiation.Login;
using EDeals.Core.Domain.Models.Authentiation.Register;
using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using EDeals.Core.Infrastructure.Shared.ExecutionContext;
using EDeals.Core.Infrastructure.TokenHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EDeals.Core.Infrastructure.Identity.Repository
{
    public class IdentityRepository : Result, IIdentityRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<IdentityRepository> _logger;
        private readonly IEmailService _emailService;
        private readonly ISendSmsService _smsService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICustomExecutionContext _executionContext;

        private const string RefreshTokenProvider = "RefreshTokenProvider";
        private const string TokenName = "RefreshToken";

        public IdentityRepository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<IdentityRepository> logger,
            IEmailService emailService,
            AppDbContext context,
            ISendSmsService smsService,
            ITokenHelper tokenHelper,
            ICustomExecutionContext executionContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _context = context;
            _smsService = smsService;
            _tokenHelper = tokenHelper;
            _executionContext = executionContext;
        }

        public async Task<ResultResponse<RegisterResponse>> CreateUserAsync(string firstName, string lastName, string userName, string email, string phoneNumber, string password)
        {
            var user = new ApplicationUser(firstName, lastName, userName, email, phoneNumber);

            var userCreationResult = await _userManager.CreateAsync(user, password);

            if (!userCreationResult.Succeeded)
            {
                _logger.LogError("Account creation failed - ", userCreationResult.Errors);
                //return userCreationResult.ToApplicationResult<RegisterResponse>();
                return BadRequest<RegisterResponse>(new ResponseError(ErrorCodes.AccountCreation, ResponseErrorSeverity.Error, RegisterMessages.CreationFailure));
            }
            
            var assigningUserRoleResult = await _userManager.AddToRoleAsync(user, RoleNames.UserRole);

            if (!assigningUserRoleResult.Succeeded)
            {
                _logger.LogError("Account role asigning failed - ", assigningUserRoleResult.Errors);
                return BadRequest<RegisterResponse>(new ResponseError(ErrorCodes.AssigningRole, ResponseErrorSeverity.Error, RegisterMessages.RoleAssigningFailure));
            }

            var userClaims = new List<Claim> 
            {
                new Claim(ClaimTypes.Role, RoleNames.UserRole),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.UserData, user.Email),
            };

            await _userManager.AddClaimsAsync(user, userClaims);

            var response = new RegisterResponse
            {
                AccessToken = _tokenHelper.CreateToken(userClaims),
                RefreshToken = await _tokenHelper.SetAuthenticationToken(user, RefreshTokenProvider, TokenName)
            };

            return Ok(response);
        }

        public async Task<ResultResponse<LoginResponse>> SignInUserAsync(string password, string? email = null, string? username = null)
        {
            var user = await GetUser(email: email, username: username);

            if (user is null)
            {
                _logger.LogError("User with email {email} / username {username} does not exist", email, username);
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

            var userClaims = await _userManager.GetClaimsAsync(user);
            
            var response = new LoginResponse
            {
                AccessToken = _tokenHelper.CreateToken(userClaims.ToList()),
                RefreshToken = await _tokenHelper.SetAuthenticationToken(user, RefreshTokenProvider, TokenName)
            };

            return Ok(response);
        }

        public async Task<ResultResponse> SignOutUserAsync()
        {
            var userId = _executionContext.UserId;

            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InternalServer, ResponseErrorSeverity.Error, GenericMessages.GenericMessage));
            }

            await _signInManager.SignOutAsync();

            return Ok();
        }

        public async Task<ResultResponse> SendEmailToken()
        {
            var userId = new Guid();

            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            if (user.EmailConfirmed)
            {
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.AlreadyConfirmed, ResponseErrorSeverity.Error, GenericMessages.AlreadyConfirmed));
            }

            if (user.ResendTokenAvailableAfter > DateTime.UtcNow)
            {
                _logger.LogError("Time out for resending digit code", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.DigitCodeTimeout, ResponseErrorSeverity.Error, GenericMessages.DigitCodeTimeout));
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            // TODO: Keep the credits for demo purpose
            //await _emailService.SendVerificationEmail(user.Email, user.FirstName, confirmationToken);

            user.ResendTokenAvailableAfter = DateTime.UtcNow.AddMinutes(1);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<ResultResponse> ConfirmUserEmail(string token)
        {
            var userId = new Guid();

            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Invalid confirmation email token");
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InternalServer, ResponseErrorSeverity.Error, GenericMessages.GenericMessage));
            }

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            
            if (!emailConfirmationResult.Succeeded)
            {
                return BadRequest(emailConfirmationResult.ToApplicationResult());
            }

            user.ResendTokenAvailableAfter = null;
            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<ResultResponse> SendPhoneCode()
        {
            var userId = new Guid();

            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            if (user.PhoneNumberConfirmed)
            {
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.AlreadyConfirmed, ResponseErrorSeverity.Error, GenericMessages.AlreadyConfirmed));
            }
            
            if (user.ResendCodeAvailableAfter > DateTime.UtcNow)
            {
                _logger.LogError("Time out for resending digit code", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.DigitCodeTimeout, ResponseErrorSeverity.Error, GenericMessages.DigitCodeTimeout));
            }

            user.DigitCode = GenerateDigitCode();
            user.ResendCodeAvailableAfter = DateTime.UtcNow.AddMinutes(1);

            // TODO: Keep the credits for demo purpose
            //await _smsService.SendSmsNotification(user.PhoneNumber, user.DigitCode);

            await _context.SaveChangesAsync();

            return Ok();
        }

        public async Task<ResultResponse> ConfirmUserPhone(string digitCode)
        {
            var userId = new Guid();

            var user = await GetUser(userId);

            if (user is null)
            {
                _logger.LogError("User with id {userId} does not exist", userId);
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            if (user.DigitCode != digitCode)
            {
                _logger.LogError("Invalid digit code");
                return BadRequest<LoginResponse>(new ResponseError(ErrorCodes.InvalidDigitCode, ResponseErrorSeverity.Error, GenericMessages.InvalidDigitCode));
            }

            user.PhoneNumberConfirmed = true;
            user.ResendCodeAvailableAfter = null;
            await _context.SaveChangesAsync();

            return Ok();
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
        private async Task<ApplicationUser?> GetUser(Guid? userId = null, string? email = null, string? username = null)
        {
            var user = new ApplicationUser();

            try
            {
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
            catch
            {
                return null;
            }
        }

        private static string GenerateDigitCode()
        {
            Random generator = new();
            return generator.Next(0, 1000000).ToString("D6");
        }
        #endregion
    }
}
