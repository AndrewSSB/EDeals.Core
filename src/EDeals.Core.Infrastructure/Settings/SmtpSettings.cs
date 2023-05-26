namespace EDeals.Core.Infrastructure.Settings
{
    public class SmtpSettings
    {
        public string FromName { get; set; } = "";
        public string From{ get; set; } = "";
        public string ApiKey { get; set; } = "";
        public string Host { get; set; } = "";
        public string Port { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
