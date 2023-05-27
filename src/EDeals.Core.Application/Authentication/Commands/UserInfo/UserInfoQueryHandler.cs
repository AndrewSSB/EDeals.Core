using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Interfaces.UserServices;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using EDeals.Core.Domain.Models.User;

namespace EDeals.Core.Application.Authentication.Commands.UserInfo
{
    public class UserInfoQueryHandler : IMediatRQueryHandler<UserInfoQuery, UserInfoResponse>
    {
        private readonly IUserService _userService;

        public UserInfoQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ResultResponse<UserInfoResponse>> Handle(UserInfoQuery request, CancellationToken cancellationToken) =>
            await _userService.GetUserInfo();
    }
}
