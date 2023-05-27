using EDeals.Core.Domain.Common.ErrorMessages;
using EDeals.Core.Domain.Common.GenericResponses.BaseResponses;
using EDeals.Core.Domain.Common.GenericResponses.ServiceResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EDeals.Core.Infrastructure.Shared.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error caught by exception handler: {exceptionMessage}", ex.Message);
                await context.Response.WriteAsync(GenericMessages.GenericMessage);
            }
            finally
            {
            }
        }
    }
}
