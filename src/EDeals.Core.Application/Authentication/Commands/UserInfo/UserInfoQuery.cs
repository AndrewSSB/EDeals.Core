using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Domain.Models.User;

namespace EDeals.Core.Application.Authentication.Commands.UserInfo
{
    public record UserInfoQuery() : IMediatRQuery<UserInfoResponse>;
}
