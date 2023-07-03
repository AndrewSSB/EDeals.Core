using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Application.Models;
using EDeals.Core.Application.Models.Authentication;
using EDeals.Core.Application.Models.UserProfile;
using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Infrastructure.Context;
using EDeals.Core.Infrastructure.Identity.Auth;
using EDeals.Core.Infrastructure.Identity.Extensions;
using EDeals.Core.Infrastructure.Identity.Repository;
using EDeals.Core.Infrastructure.Settings;
using EDeals.Core.Infrastructure.Shared.ExecutionContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EDeals.Core.Infrastructure.Services.UserServices
{
    public class UserService : Result, IUserService
    {
        private readonly ICustomExecutionContext _executionContext; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly HttpClient _client;
        private readonly ApplicationSettings _appSettings;


        public UserService(ICustomExecutionContext executionContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context,
            IHttpClientFactory httpClientFactory, 
            IOptions<ApplicationSettings> appSettings)
        {
            _executionContext = executionContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _client = httpClientFactory.CreateClient();
            _appSettings = appSettings.Value;
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
                UserId = userId
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

            user.UserName += DateTime.UtcNow.ToString("ZIddLUNAMMANyyyy");
            user.IsDeleted = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
            }

            return result.ToApplicationResult();
        }

        public async Task<ResultResponse> BlockUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        
        public async Task<ResultResponse> UnBlockUser(Guid userId)
        {
            var user = await _context.Users.IgnoreQueryFilters().Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                user.IsDeleted = false;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        public async Task<ResultResponse> UpdateUser(UpdateUserModel model, string token)
        {
            var user = await _userManager.FindByIdAsync(_executionContext.UserId.ToString());

            if (user == null)
            {
                return BadRequest(new ResponseError(ErrorCodes.UserDoesNotExists, ResponseErrorSeverity.Error, GenericMessages.UserDoesNotExists));
            }

            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            
            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
                user.EmailConfirmed = false;
            }
            
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = false;
            }
            
            if (!string.IsNullOrEmpty(model.Username))
            {
                user.UserName = model.Username;
            }

            await _context.SaveChangesAsync();

            SendDataToCatalog(model, token);
            return Ok();
        }

        public async Task<ResultResponse<List<UserInfoResponse>>> GetUsers(string username)
        {
            var users = _context.Users.Where(x => x.Id != _executionContext.UserId);
                
            if (!string.IsNullOrEmpty(username))
            {
                users = users.Where(x => x.UserName.Contains(username));
            }
                
                
            var response = await users.Select(user => new UserInfoResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsEmailVerified = user.EmailConfirmed,
                IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
            })
            .ToListAsync();

            return Ok(response);
        }
        
        public async Task<ResultResponse<List<UserInfoResponse>>> GetUsersAdmin()
        {
            var users = _context.Users.IgnoreQueryFilters().Where(x => x.Id != _executionContext.UserId);
                      
            var response = await users.Select(user => new UserInfoResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsEmailVerified = user.EmailConfirmed,
                IsPhoneNumberVerified = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                UserId = user.Id,
            })
            .ToListAsync();

            return Ok(response);
        }

        private async Task SendDataToCatalog(UpdateUserModel model, string jwtToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwtToken);

            var modelJson = JsonConvert.SerializeObject(model);

            HttpContent request = new StringContent(modelJson, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_appSettings.ApiProtocol}://{_appSettings.CatalogBaseUrl}/api/userinfo", request);

            if (!response.IsSuccessStatusCode)
            {
                //....
            }
        }
    }
}
