using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
        {
            string apiKey = _configuration.GetConnectionString("SEND_GRID_KEY");
            SendGridClient client = new SendGridClient(apiKey);
            EmailAddress from = new EmailAddress("test@studento.cz", "Sender");
            EmailAddress to = new EmailAddress(emailTo, "Example User");
            string plainTextContent = "and easy to do anywhere, even with C#";
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            Response response = await client.SendEmailAsync(msg);
        }
    }
}
