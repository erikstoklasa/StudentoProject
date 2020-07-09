using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SchoolGradebook.Pages.Admin.Import
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IFormFile FileUpload { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (FileUpload.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                using var stream = System.IO.File.Create(filePath);
                await FileUpload.CopyToAsync(stream);
                stream.Close();
                Response<BlobContentInfo> response = null;
                try
                {
                    response = await UploadToAzureBlobStorage(filePath);
                    //TODO:log filename into db
                }
                catch (RequestFailedException e){
                    ViewData["status"] = e.ToString();
                }
                if(response != null)
                {
                    ViewData["status"] = "OK";
                }
            }
            await SendEmail(
                "me@erikstoklasa.cz",
                "Importování začalo",
                "<h1>Status importování studentů</h1><p>Přijali jsme Vaše záznamy o studentech, jakmile budou naimportovány, tak Vás informujeme emailem. Může to trvat několik hodin.</p>");

            return Page();
        }
        public async Task<Response<BlobContentInfo>> UploadToAzureBlobStorage(string filePath)
        {
            string connectionString = _configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("studentimport");
            BlobClient blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}.csv");
            using FileStream uploadFileStream = System.IO.File.OpenRead(filePath);
            return await blobClient.UploadAsync(uploadFileStream, true);
        }
        public async Task<SendGrid.Response> SendEmail(string emailTo, string subject, string content)
        {
            string apiKey = _configuration.GetConnectionString("SEND_GRID_KEY");
            SendGridClient client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("mailer@studento.cz", "Studento import"),
                Subject = subject
            };
            msg.AddContent(MimeType.Html, content);
            msg.AddTo(emailTo, "Admin");
            //SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlMessage);
            SendGrid.Response r = await client.SendEmailAsync(msg);
            return r;
        }
    }
}
