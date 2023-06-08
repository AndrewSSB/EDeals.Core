﻿namespace EDeals.Core.API
{
    internal static class ApiExtensions
    {
        public static void AddApplicationSettings(WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("appsettings.json", false, true);
            builder.Configuration.AddJsonFile("appsettings.Local.json", true, true);
            builder.Configuration.AddEnvironmentVariables();
        }
    }
}
