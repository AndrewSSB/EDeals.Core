namespace EDeals.Core.Application.Models.Authentication.Login
{
    public sealed class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
