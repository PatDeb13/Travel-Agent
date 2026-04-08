using System.Net;
using System.Net.Mail;
using Travel_Agent.Auth;

namespace Travel_Agent.Auth.AuthServices
{
   public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("c80f378df9d343", "f65e83f2435358"),
            EnableSsl = true
        };

        var message = new MailMessage(
            "hello@demomailtrap.co",
            toEmail,
            subject,
            body
        );

        await client.SendMailAsync(message);
    
     
        }
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        
    }
}
