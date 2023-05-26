namespace EDeals.Core.Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string to, string name, string digitCode, CancellationToken cancellationToken = default);
    }
}
