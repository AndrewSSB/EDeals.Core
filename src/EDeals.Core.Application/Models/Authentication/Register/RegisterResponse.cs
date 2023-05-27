namespace EDeals.Core.Application.Models.Authentication.Register
{
    public sealed class RegisterResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
