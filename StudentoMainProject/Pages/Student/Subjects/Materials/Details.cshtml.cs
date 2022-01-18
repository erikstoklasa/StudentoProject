using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeTypes;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.Models;
using StudentoMainProject.Services;
using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Student.Subjects.Materials
{
    public class DetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly SubjectMaterialService subjectMaterialService;
        private readonly LogItemService logItemService;
        private readonly StudentService studentService;

        public DetailsModel(IConfiguration configuration,
                            SubjectMaterialService subjectMaterialService,
                            LogItemService logItemService,
                            StudentService studentService,
                            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            this.subjectMaterialService = subjectMaterialService;
            this.logItemService = logItemService;
            this.studentService = studentService;
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public SubjectMaterial SubjectMaterial { get; set; }
        public IPAddress IPAddress { get; set; }
        public string UserId { get; private set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int studentId = await studentService.GetStudentId(UserId);

            SubjectMaterial = await subjectMaterialService.GetMaterialAsync((Guid)id);

            if (SubjectMaterial == null)
            {
                return NotFound();
            }

            string connectionString = _configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("subjectmaterials");
            BlobClient blobClient = containerClient.GetBlobClient($"{id}{SubjectMaterial.FileExt}");
            var downloadFileStream = new MemoryStream();
            try
            {
                Response r = await blobClient.DownloadToAsync(downloadFileStream);
                await logItemService.Log(
                new LogItem
                {
                    EventType = "SubjectMaterial",
                    Timestamp = DateTime.UtcNow,
                    UserAuthId = UserId,
                    UserId = studentId,
                    UserRole = "student",
                    IPAddress = IPAddress.ToString(),
                });
            }
            catch (Exception)
            {
                return NotFound("Soubor nenalezen");
            }
            downloadFileStream.Position = 0;
            return File(downloadFileStream,
                MimeTypeMap.GetMimeType(SubjectMaterial.FileExt),
                $"{SubjectMaterial.Name}{SubjectMaterial.FileExt}");
        }
    }
}
