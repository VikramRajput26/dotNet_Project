using System.Net.Mail;
using System.Net;

namespace MyProjectJWT.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["SMTPConfig:Host"])
            {
                Port = int.Parse(_configuration["SMTPConfig:Port"]),
                Credentials = new NetworkCredential(_configuration["SMTPConfig:UserName"], _configuration["SMTPConfig:Password"]),
                EnableSsl = bool.Parse(_configuration["SMTPConfig:EnableSSL"]),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SMTPConfig:SenderAddress"], _configuration["SMTPConfig:SenderDisplayName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = bool.Parse(_configuration["SMTPConfig:IsBodyHTML"]),
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }


    }
}
