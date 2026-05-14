using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Travel_Agent.Services
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string body, bool isHtml=true);

        
    }
    public class EmailService:IEmailService
    {
        
        public EmailService()
        {
            
        }
        public class EmailDTO
        {
            public string Host;
            public int Port;
            public string User;
            public string Email;
            public string Name;
            public string Password;
        }

        public async Task SendEmail(string toEmail, string subject, string body, bool isHtml = true)
        {
            if(string.IsNullOrWhiteSpace(toEmail))
             throw new ArgumentException("Recipient emai address cannot be null or empty");
             var config = new EmailDTO()
             {
                 Host= "sandbox.smtp.mailtrap.io",
                 Port = 2525,
                 User= "c80f378df9d343",
                 Email ="noreply@TravelAgent.com",
                 Name=  "Travel Agent",
                 Password =  "f65e83f2435358"
             };

             using var message=new MailMessage();
             message.From =new MailAddress(config.Email.Trim());
             message.To.Add(toEmail);
             message.Subject = subject;
             message.Body =body;
             message.IsBodyHtml=isHtml;

             using var client = new SmtpClient(config.Host.Trim(), config.Port)
             {
                 Credentials = new NetworkCredential(config.User.Trim(), config.Password),
                 EnableSsl =true,
                 Timeout =30000
             };
                await client.SendMailAsync(message);
        }
    }     
}



            
    
