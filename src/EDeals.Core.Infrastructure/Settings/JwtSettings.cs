﻿namespace EDeals.Core.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string ValidAudience { get; set; } = "";
        public string ValidIssuer { get; set; } = "";
        public string Secret { get; set; } = "";
        public string Expiration { get; set; } = "";
    }
}
