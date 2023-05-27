namespace EDeals.Core.Domain.Models.Authentiation.Login
{
    public sealed class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
