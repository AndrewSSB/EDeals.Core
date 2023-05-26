using EDeals.Core.Application.Interfaces.Email;
using EDeals.Core.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Net;
using System.Net.Mail;
using Task = System.Threading.Tasks.Task;

namespace EDeals.Core.Infrastructure.Services.EmailServices
{
    public class BaseEmailService : IBaseEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpSettings _smtpSettings;

        public BaseEmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpSettings)
        {
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailUsingApi(string to, string subject, string htmlContent)
        {
            //Sendinblue
            //var configuration = Configuration.Default;
            //configuration.ApiKey.Add("api-key", _smtpSettings.ApiKey);
            //var apiInstance = new TransactionalEmailsApi(configuration);

            //var sendSmtpEmail = new SendSmtpEmail(
            //    sender: new SendSmtpEmailSender(email: _smtpSettings.From),
            //    to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(email: to) },
            //    htmlContent: htmlContent,
            //    subject: subject
            //    );

            //try
            //{
            //    var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            //    _logger.LogInformation("{MessageId}", result.MessageId);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("Exception when calling TransactionalEmailsApi.SendTransacEmail: {Message}", ex.Message);
            //}

            var client = new SendGridClient(_smtpSettings.ApiKey);
            var from = new EmailAddress(_smtpSettings.From, _smtpSettings.FromName);
            var receiver = new EmailAddress(to);

            try
            {
                var msg = MailHelper.CreateSingleEmail(from, receiver, subject, "", htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception when calling TransactionalEmailsApi.SendTransacEmail: {Message}", ex.Message);
            }
        }

        public async Task SendEmailUsingSmtp(string to, string subject, string htmlContent, CancellationToken cancellationToken = default)
        {
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(_smtpSettings.From, _smtpSettings.FromName),
                Subject = subject,
                Body = htmlContent,
            };

            message.To.Add(new MailAddress(to));

            using var client = new SmtpClient(_smtpSettings.Host, int.Parse(_smtpSettings.Port));

            client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
            client.EnableSsl = true;

            await client.SendMailAsync(message, cancellationToken);
        }
    }
}
