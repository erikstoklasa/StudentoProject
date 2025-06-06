﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SchoolGradebook.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SchoolGradebook.Pages.Admin.Import
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly StudentService studentService;
        private readonly AdminService adminService;

        public IFormFile FileUpload { get; set; }
        private string UserId { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, StudentService studentService, AdminService adminService)
        {
            _configuration = configuration;
            this.studentService = studentService;
            this.adminService = adminService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var sr = new StreamReader(fs, Encoding.UTF8);

                string line = String.Empty;
                List<Models.Student> students = new();
                int schoolId = (await adminService.GetAdminByUserAuthId(UserId)).SchoolId;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] entries = line.Split(',');
                    if (entries.Length != 6)
                    {
                        ViewData["status"] = "Nesprávný formát souboru";
                        Response<BlobContentInfo> response = null;
                        try
                        {
                            response = await UploadToAzureBlobStorage(filePath);
                        }
                        catch (RequestFailedException)
                        {
                        }
                        return Page();
                    }
                    students.Add(new Models.Student()
                    {
                        FirstName = entries[0],
                        LastName = entries[1],
                        Birthdate = DateTime.Parse(entries[2]),
                        PersonalIdentifNumber = entries[3],
                        Email = entries[4],
                        ClassId = string.IsNullOrWhiteSpace(entries[5]) ? null : int.Parse(entries[5]),
                        SchoolId = schoolId
                    });
                }
                var success = await studentService.AddStudentRangeAsync(students);
                if (!success)
                {
                    ViewData["status"] = "Studenti nemají všechny požadované hodnoty, nebo je nešlo vložit do databáze";
                    return Page();
                }

            }
            //await SendEmail(
            //    "me@erikstoklasa.cz",
            //    "Importování začalo",
            //    "<h1>Status importování studentů</h1><p>Přijali jsme Vaše záznamy o studentech, jakmile budou naimportovány, tak Vás informujeme emailem. Může to trvat několik hodin.</p>");
            ViewData["status"] = "OK";
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
            SendGrid.Response r = await client.SendEmailAsync(msg);
            return r;
        }
    }
}
