using System.Net;
using System.Net.Mail;
using Travel_Agent.Auth;

namespace Travel_Agent.Auth.AuthServices
{
    public class EmailService:IEmailService
    {
        public async Task<ResponseModelAuth<string>> EmailSettings(string toEmail , string subject, string body, string EmployeeId)
        {
            using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("c80f378df9d343", "f65e83f2435358"),
                EnableSsl = true
            };
            var message = new MailMessage(
                "hello@demomailtrap.co",
                "obamuyipatience13@gmail.com",
                "Hello World",
                "$This is your EmployeeId: {employeeId}"
            );

            await client.SendMailAsync(message);

            return new ResponseModelAuth<string>
            {
                IsSuccessful=true,
                Message= "Email sent successful",
                Data = EmployeeId
            };
        }
    }

    public interface IEmailService
    {
        Task<ResponseModelAuth<string>> EmailSettings(string toEmail , string subject, string body, string EmployeeId);
    }
}
