using EDeals.Core.Application.Interfaces.SMS;
using EDeals.Core.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace EDeals.Core.Infrastructure.Services.SmsService
{
    public class SendSmsService : ISendSmsService
    {
        private readonly TwilioSettings _twilioSettings;
        private readonly ILogger<SendSmsService> _logger;

        public SendSmsService(IOptions<TwilioSettings> twilioSettings, ILogger<SendSmsService> logger)
        {
            _twilioSettings = twilioSettings.Value;
            _logger = logger;
        }

        public async Task SendSmsNotification(string phoneNumber, string digitCode)
        {
            var accountSid = _twilioSettings.AccountSid;
            var authToken = _twilioSettings.AuthToken;
            
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber($"{phoneNumber}");
            var from = new PhoneNumber(_twilioSettings.FromNumber);

            try
            {
                var message = await MessageResource.CreateAsync(
                        to: to,
                        from: from,
                        body: $"E-Deals authentification code: {digitCode}. Don't share it with anyone, ever.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message: {exceptionMessage}", ex.Message);
            }
        }
    }
}
