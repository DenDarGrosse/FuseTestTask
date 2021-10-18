using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace test_fuse.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string mailLogin;
        private readonly string mailPassword;

        public EmailService(IConfiguration configuration) 
        {
            mailLogin = configuration["Mail:login"];
            mailPassword = configuration["Mail:password"];
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(mailLogin);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(mailLogin, mailPassword),
                EnableSsl = true,
            };
            return smtpClient.SendMailAsync(message);
        }
    }
}
