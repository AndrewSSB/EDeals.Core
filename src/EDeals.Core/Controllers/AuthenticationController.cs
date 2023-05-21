using EDeals.Core.Application.Authentication.Commands.Login;
using EDeals.Core.Application.Authentication.Commands.Register;
using EDeals.Core.Domain.Models.Authentiation.Login;
using EDeals.Core.Domain.Models.Authentiation.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EDeals.Core.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<ActionResult<string>> Register([FromBody] RegisterModel model)
        {
            var command = new RegisterCommand(model.FirstName, model.LastName, model.UserName, model.Email, model.PhoneNumber, model.Password);

            return Ok(await _mediator.Send(command));
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            var command = new LoginCommand(model.Email, model.UserName, model.Password);

            return Ok(await _mediator.Send(command));
        }
    }
}
