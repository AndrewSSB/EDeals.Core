namespace EDeals.Core.Application.Models.UserProfile
{
    public class UserInfoResponse
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsPhoneNumberVerified { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
