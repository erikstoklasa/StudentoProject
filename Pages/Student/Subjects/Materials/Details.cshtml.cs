using Azure;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeTypes;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Student.Subjects.Materials
{
    public class DetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly SubjectMaterialService subjectMaterialService;

        public DetailsModel(IConfiguration configuration, SubjectMaterialService subjectMaterialService)
        {
            _configuration = configuration;
            this.subjectMaterialService = subjectMaterialService;
        }

        public SubjectMaterial SubjectMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
