namespace EDeals.Core.Domain.Models.Authentiation.Register
{
    public sealed class RegisterResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
