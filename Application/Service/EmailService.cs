using Contracts.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Application.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var mail = "bookinghotelwebsite11@gmail.com";
            var passwordApp = "nxqn yunh zmbn kjng";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(mail, passwordApp),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(mail, toEmail, subject, message);
            await client.SendMailAsync(mailMessage);
        }
    }
}
