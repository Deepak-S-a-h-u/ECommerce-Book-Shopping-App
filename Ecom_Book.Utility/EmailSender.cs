using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Book.Utility
{
    public class EmailSender : IEmailSender
    {
        private emailSettings _emailSettings { get; }
        public EmailSender(IOptions<emailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }
        public async Task Execute(string email, string subject, string messege)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
                MailMessage Mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "My Email Name")
                };
                    Mail.To.Add(new MailAddress(toEmail));
                    Mail.CC.Add(new MailAddress(_emailSettings.CcEmail));
                    Mail.Subject = "Book Shopping Project:" + subject;
                    Mail.Body = messege;
                    Mail.IsBodyHtml = true;
                    Mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(Mail);
                }
                
            }
            catch(Exception ex)
            {
                string str = ex.Message;
            }
            
        }
    }
}
