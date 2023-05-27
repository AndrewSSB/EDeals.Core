using EDeals.Core.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EDeals.Core.Infrastructure.Shared.TokenExtensions
{
    public static class TokenManager
    {
        public static string CreateSecurityToken(JwtSettings settings, List<Claim> claims)
        {
            var secret = Convert.FromBase64String(settings.Secret);
            var key = new SymmetricSecurityKey(secret);
            var issuer = settings.ValidIssuer;
            var audience = settings.ValidAudience;
            var duration = DateTime.UtcNow.AddHours(int.Parse(settings.Expiration));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = duration,
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CreateEncodedJwt(tokenDescriptor);
        }

        public static string CreateSecurityTokenUnixTimeExpiry(JwtSettings settings, List<Claim> claims, long expiryInUnixTimeSeconds)
        {
            var secret = Convert.FromBase64String(settings.Secret);
            var key = new SymmetricSecurityKey(secret);
            var issuer = settings.ValidIssuer;
            var audience = settings.ValidAudience;
            var duration = DateTimeOffset.FromUnixTimeSeconds(expiryInUnixTimeSeconds).UtcDateTime;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = duration,
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Audience = audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CreateEncodedJwt(tokenDescriptor);
        }

        public static Claim? ExtractValueFromToken(JwtSettings settings, string token, string claimType)
        {
            var claims = ExtractValuesFromToken(settings, token);

            return claims.Where(c => c.Type == claimType).FirstOrDefault();
        }

        public static List<Claim> ExtractValuesFromToken(JwtSettings settings, string token)
        {
            token = RemoveDefaultAuthenticationFromToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = GetValidationParameters(settings);

            var validation = tokenHandler.ValidateToken(token, validationParams, out var _);

            return validation.Claims.ToList();
        }

        public static async Task<bool> ValidateToken(JwtSettings settings, string token)
        {
            token = RemoveDefaultAuthenticationFromToken(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParams = GetValidationParameters(settings);

            var validation = await tokenHandler.ValidateTokenAsync(token, validationParams);

            return validation.IsValid;
        }

        private static TokenValidationParameters GetValidationParameters(JwtSettings settings) =>
            new()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = settings.ValidIssuer,
                ValidAudience = settings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(settings.Secret))
            };

        private static string RemoveDefaultAuthenticationFromToken(string token)
        {
            if (token.StartsWith("Bearer"))
            {
                token = token.Replace("Bearer ", "");
            }

            return token;
        }
    }
}
