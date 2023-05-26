using EDeals.Core.Application.Interfaces.Mediator;

namespace EDeals.Core.Application.Authentication.Commands.ConfirmPhone
{
    public record ConfirmPhoneCommand(string DigitCode) : IMediatRCommand;
}
