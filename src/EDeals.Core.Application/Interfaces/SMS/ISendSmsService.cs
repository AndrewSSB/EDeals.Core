namespace EDeals.Core.Application.Interfaces.SMS
{
    public interface ISendSmsService
    {
        Task SendSmsNotification(string phoneNumber, string digitCode);
    }
}
