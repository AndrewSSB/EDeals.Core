using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.Authentication.Login;

namespace EDeals.Core.Application.Authentication.Commands.Login
{
    public record LoginCommand(string? Email, string? UserName, string Password) : IMediatRCommand<LoginResponse>;
}
