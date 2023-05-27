namespace EDeals.Core.Domain.Models.Authentiation.Login
{
    public sealed class LoginModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}
