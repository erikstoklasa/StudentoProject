using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SchoolGradebook.Areas.Admin.Pages.Import
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
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
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
    }
}
