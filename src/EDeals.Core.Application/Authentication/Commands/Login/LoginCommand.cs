using MediatR;

namespace EDeals.Core.Application.Authentication.Commands.Login
{
    public record LoginCommand(string? Email, string? UserName, string Password) : IRequest<string?>;
}
