using EDeals.Core.Application.Interfaces.Mediator;

namespace EDeals.Core.Application.Authentication.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Token) : IMediatRCommand;
}
