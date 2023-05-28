using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.UserProfile;

namespace EDeals.Core.Application.User.Queries.UserInfo
{
    public record UserInfoQuery() : IMediatRQuery<UserInfoResponse>;
}
