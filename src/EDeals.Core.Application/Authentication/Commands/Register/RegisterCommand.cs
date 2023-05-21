using MediatR;

namespace EDeals.Core.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string FirstName, string LastName, string UserName, string Email, string PhoneNumber, string Password) : IRequest<string?>;
}
