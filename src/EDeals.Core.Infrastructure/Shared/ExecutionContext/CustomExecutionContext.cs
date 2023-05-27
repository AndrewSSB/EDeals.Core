using EDeals.Core.Infrastructure.Settings;
using EDeals.Core.Infrastructure.Shared.TokenExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace EDeals.Core.Infrastructure.Shared.ExecutionContext
{
    public interface ICustomExecutionContext
    {
        public Guid UserId { get; }
        public DateTime ExpiresAt { get; }
    }

    public class CustomExecutionContext : ICustomExecutionContext
    {
        private readonly JwtSettings _settings;

        public CustomExecutionContext(IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            
            try
            {
                var authorizationHeader = httpContextAccessor.HttpContext?.Request?.Headers?.Authorization;
                if (authorizationHeader.HasValue && !StringValues.IsNullOrEmpty(authorizationHeader.Value))
                {
                    var nameIdentifier = TokenManager.ExtractValueFromToken(_settings, authorizationHeader.Value!, ClaimTypes.NameIdentifier);

                    if (Guid.TryParse(nameIdentifier?.Value, out Guid parsedResult))
                    {
                        UserId = parsedResult;
                    }

                    var expiration = TokenManager.ExtractValueFromToken(_settings, authorizationHeader.Value!, ClaimTypes.Expiration);

                    if (DateTime.TryParse(expiration?.Value, out DateTime parsedExpiration))
                    {
                        ExpiresAt = parsedExpiration.ToUniversalTime();
                    }
                }
            }
            catch { }
        }

        public Guid UserId { get; set; } = Guid.Empty;
        public DateTime ExpiresAt { get; set; } = DateTime.MinValue;
    }
}
