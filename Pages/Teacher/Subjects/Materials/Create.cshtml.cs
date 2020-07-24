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
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Subjects.Materials
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly TeacherService teacherService;
        private readonly SubjectService subjectService;
        private readonly SubjectMaterialService subjectMaterialService;

        public List<SelectListItem> SubjectSelectList { get; set; }
        public string UserId { get; set; }

        public CreateModel(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, TeacherService teacherService, SubjectService subjectService, SubjectMaterialService subjectMaterialService)
        {
            _configuration = configuration;
            SubjectSelectList = new List<SelectListItem>();
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.subjectMaterialService = subjectMaterialService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }
        [BindProperty(SupportsGet = true)]
        public int? SubjectInstanceId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (SubjectInstanceId == null)
            {
                return NotFound();
            }

            SubjectInstance s = await subjectService.GetSubjectInstanceAsync((int)SubjectInstanceId);
            
            if (s == null)
            {
                return NotFound();
            }

            SubjectSelectList.Add(new SelectListItem(s.SubjectType.Name.ToString(), s.SubjectTypeId.ToString()));
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
                    SubjectMaterial.TeacherId = await teacherService.GetTeacherId(UserId);
                    //Validate subject type id access
                    SubjectMaterial.Added = DateTime.UtcNow;
                    SubjectMaterial.FileExt = Path.GetExtension(FileUpload.FileName);
                    await subjectMaterialService.AddMaterialAsync(SubjectMaterial);
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
            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectInstanceId }");
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
