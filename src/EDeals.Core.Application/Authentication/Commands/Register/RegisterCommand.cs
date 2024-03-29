﻿using EDeals.Core.Application.Interfaces.Mediator;
using EDeals.Core.Application.Models.Authentication.Register;

namespace EDeals.Core.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string FirstName,
        string LastName,
        string UserName,
        string Email,
        string PhoneNumber,
        string Password) : IMediatRCommand<RegisterResponse>;
}
