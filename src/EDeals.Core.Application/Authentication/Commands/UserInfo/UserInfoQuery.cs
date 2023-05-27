using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.UserProfile;

namespace EDeals.Core.Application.Authentication.Commands.UserInfo
{
    public record UserInfoQuery() : IMediatRQuery<UserInfoResponse>;
}
