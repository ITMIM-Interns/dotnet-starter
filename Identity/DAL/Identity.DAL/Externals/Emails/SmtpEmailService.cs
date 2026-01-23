using Identity.BLL.Abstractions.Externals;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Identity.DAL.Implementations.Externals.Emails
{
    public sealed class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _from;

        public SmtpEmailService(IConfiguration configuration)
        {
            _from = configuration["Email:From"];
            _smtpClient = new SmtpClient
            {
                Host = configuration["Email:Smtp:Host"],
                Port = int.Parse(configuration["Email:Smtp:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                configuration["Email:Smtp:Username"],
                configuration["Email:Smtp:Password"])
            };
        }

        public async Task SendAsync(string to, string body, string subject)
        {
            var mail = new MailMessage(_from, to, subject, body)
            {
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mail);
        }
    }
}
