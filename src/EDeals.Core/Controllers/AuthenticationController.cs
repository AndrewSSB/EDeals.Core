using EDeals.Core.API.Extensions;
using EDeals.Core.Application.Authentication.Commands.ConfirmEmail;
using EDeals.Core.Application.Authentication.Commands.ConfirmPhone;
using EDeals.Core.Application.Authentication.Commands.Login;
using EDeals.Core.Application.Authentication.Commands.Logout;
using EDeals.Core.Application.Authentication.Commands.Register;
using EDeals.Core.Application.Authentication.Commands.SendCode;
using EDeals.Core.Application.Authentication.Commands.SendToken;
using EDeals.Core.Application.Models.Authentication.Login;
using EDeals.Core.Application.Models.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EDeals.Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstName"> Required string </param>
        /// <param name="LastName"> Required string </param>
        /// <param name="UserName"> Required string </param>
        /// <param name="Email"> Required string </param>
        /// <param name="PhoneNumber"> Required string </param>
        /// <param name="Password"> Required </param>
        /// <returns> Access Token </returns>
        /// <remarks>
        /// Sample request:
        ///     POST /register
        ///     {
        ///         "FirstName": "John"
        ///         "LastName": "Doe"
        ///         "UserName": "username_"
        ///         "Email": "sample@gmail.com"
        ///         "PhoneNumber": "+40741498723"
        ///         "Password": "Parola123!"
        ///     }
        /// <response code="200">Returns access token</response>
        /// <response code="400">If a field is null or invalid</response>
        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterModel model)
        {
            var command = new RegisterCommand(model.FirstName, model.LastName, model.UserName, model.Email, model.PhoneNumber, model.Password);

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginModel model)
        {
            var command = new LoginCommand(model.Email, model.UserName, model.Password);

            return ControllerExtension.Map(await _mediator.Send(command));
        }
        
        [HttpPost("logout")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Logout()
        {
            var command = new LogoutCommand();

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpPost("validate-email")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendEmailConfiramtionToken()
        {
            var command = new SendTokenCommand();

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpPost("confirm-email/{token}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmEmail(string token)
        {
            var command = new ConfirmEmailCommand(token);

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpPost("validate-phone")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SendPhoneConfirmationCode()
        {
            var command = new SendCodeCommand();

            return ControllerExtension.Map(await _mediator.Send(command));
        }

        [HttpPost("confirm-phone/{digitCode}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmPhone(string digitCode)
        {
            var command = new ConfirmPhoneCommand(digitCode);

            return ControllerExtension.Map(await _mediator.Send(command));
        }
    }
}
