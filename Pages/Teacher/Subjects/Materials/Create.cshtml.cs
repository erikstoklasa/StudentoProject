using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MimeTypes;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Subjects.Materials
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly IConfiguration _configuration;
        private readonly Analytics _analytics;
        public List<SelectListItem> SubjectSelectList { get; set; }

        public CreateModel(SchoolContext context, IConfiguration configuration, Analytics analytics)
        {
            _context = context;
            _configuration = configuration;
            SubjectSelectList = new List<SelectListItem>();
            _analytics = analytics;
        }

        public async Task<IActionResult> OnGetAsync(int? SubjectInstanceId)
        {
            if (SubjectInstanceId == null)
            {
                return NotFound();
            }

            SubjectInstance s = await _analytics.GetSubjectAsync((int)SubjectInstanceId);
            
            if (s == null)
            {
                return NotFound();
            }

            SubjectSelectList.Add(new SelectListItem(s.Name, s.Id.ToString()));
            return Page();
        }
        public IFormFile FileUpload { get; set; }
        [BindProperty]
        public SubjectMaterial SubjectMaterial { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Guid guid = Guid.NewGuid();

            if (FileUpload.Length > 0)
            {
                string filePath = Path.GetTempFileName();

                using FileStream stream = System.IO.File.Create(filePath);
                await FileUpload.CopyToAsync(stream);
                stream.Close();
                Response<BlobContentInfo> response = null;
                try
                {
                    response = await UploadToAzureBlobStorage(guid, filePath, Path.GetExtension(FileUpload.FileName));

                    SubjectMaterial.Id = guid;
                    SubjectMaterial.Added = DateTime.UtcNow;
                    SubjectMaterial.FileExt = Path.GetExtension(FileUpload.FileName);
                    _context.SubjectMaterials.Add(SubjectMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (RequestFailedException e)
                {
                    ViewData["status"] = e.ToString();
                }
                if (response != null)
                {
                    ViewData["status"] = "OK";
                }
            }
            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectMaterial.SubjectTypeId }");
        }
        public async Task<Response<BlobContentInfo>> UploadToAzureBlobStorage(Guid id, string filePath, string fileExt)
        {
            string connectionString = _configuration.GetConnectionString("AZURE_STORAGE_CONNECTION_STRING");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("subjectmaterials");
            using FileStream uploadFileStream = System.IO.File.OpenRead(filePath);
            BlobClient blobClient = containerClient.GetBlobClient($"{id}{fileExt}");
            return await blobClient.UploadAsync(uploadFileStream, true);
        }
    }
}
