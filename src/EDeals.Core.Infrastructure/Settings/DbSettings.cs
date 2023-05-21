namespace EDeals.Core.Infrastructure.Settings
{
    public class DbSettings
    {
        public string MySqlConnectionString { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Endpoint { get; set; } = "";
        public int MaxPoolSize { get; set; }
    }
}
