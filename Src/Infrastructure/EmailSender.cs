using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Coreapi.Infrastructure
{
    public class EmailSender : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        public static Task Execute(string subject, string message, string email)
        {
            var client = new SmtpClient("mail.asampro.ir", 25)
            {
                Credentials = new NetworkCredential("AsAmPro@asampro.ir", "1qazxsw2"),
                //UseDefaultCredentials = false,

            };

            return client.SendMailAsync(
                new MailMessage(
                    new MailAddress("AsAmPro@asampro.ir", "AsAmPro"),
                     new MailAddress(email, email))
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = message
                }
            );
        }
    }
}
