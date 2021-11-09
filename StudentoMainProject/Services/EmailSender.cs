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
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Configuration.GetConnectionString("SEND_GRID_KEY"), subject, message, email);
        }

        public Task<Response> SendEmailWithResponseAsync(string email, string subject, string message)
        {
            return Execute(Configuration.GetConnectionString("SEND_GRID_KEY"), subject, message, email);
        }

        public Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("mailer@studento.cz"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message,
                ReplyTo = new EmailAddress("erik@studento.cz")
            };
            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
