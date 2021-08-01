using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaMovie.Services
{
    public static class SendGridApi
    {
       public static async Task<bool> Execute(string userEmail,string userName,string plainTextContent,string htmlContent,string subject)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Abdallah");
            
            var to = new EmailAddress(userEmail, userName);
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return await Task.FromResult(true);
        }
    }
}
