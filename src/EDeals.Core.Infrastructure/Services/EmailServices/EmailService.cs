using EDeals.Core.Application.Interfaces.Email;
using EDeals.Core.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace EDeals.Core.Infrastructure.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IBaseEmailService _sendinBlueService;
        private readonly ApplicationSettings _appSettings;

        public EmailService(ILogger<EmailService> logger, IBaseEmailService sendinBlueService, IOptions<ApplicationSettings> appSettings)
        {
            _sendinBlueService = sendinBlueService;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task SendVerificationEmail(string to, string name, string token, CancellationToken cancellationToken)
        {
            var filename = "wwwroot/EmailTemplates/VerificationEmail.html";

            var template = await LoadTemplate(filename, cancellationToken);

            if (template == null) return;

            template = template
                .Replace("{name}", name)
                .Replace("{image_url}", _appSettings.LogoUrl)
                .Replace("{confirmation_link}", $"https://{_appSettings.BaseUrl}/api/authentication/confirm-email/{token}");

            await _sendinBlueService.SendEmailUsingApi(to, "Verification code", template);
            //await _sendinBlueService.SendEmailUsingSmtp(to, "Verification code", template);
        }

        private async Task<string?> LoadTemplate(string filename, CancellationToken cancellationToken = default)
        {
            //var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //if (string.IsNullOrEmpty(basePath))
            //{
            //    return null;
            //}

            try
            {
                //var webRootPath = Path.Combine(basePath, "wwwroot", filename);

                _logger.LogInformation("File path {filename}", filename);

                var templateSupport = await File.ReadAllTextAsync(filename, cancellationToken);

                return templateSupport;
            }
            catch
            {
                _logger.LogError("Can't load the template, check if the path is correct");
                return null;
            }
        }
    }
}
