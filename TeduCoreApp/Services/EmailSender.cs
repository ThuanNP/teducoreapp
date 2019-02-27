using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TeduCoreApp.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //TODO: Send email
            SmtpClient client = new SmtpClient(configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(configuration["MailSettings:UserName"], configuration["MailSettings:Password"]),
            };
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(configuration["MailSettings:FromEmail"], configuration["MailSettings:FromName"]),
                Body = message,
                Subject = subject,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
