using FHP.infrastructure.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FHP.utilities.Utility;
using Microsoft.AspNetCore.Hosting;

namespace FHP.services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostingEnvironment _env;
        public FileUploadService(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<bool> DeleteIFormPdfAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            try
            {
                string fullPath = Path.Combine(_env.WebRootPath, "Attachments", filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
       
        }

        public async Task<string> UploadIFormFileAsync(IFormFile file)
        {
            string uploadsFolder = string.Empty;

            string uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {
                if (file.Length > 60 * 1024 * 1024) // 60MB in bytes
                {
                    throw new ArgumentException("File size exceeds the limit of 60MB.");
                }

                string fileExtension = Path.GetExtension(file.FileName);

                uploadsFolder = FileExtensionMap.GetCategoryForExtension(fileExtension.ToLower());


                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                string filePath = Path.Combine(_env.WebRootPath, "Attachments", uploadsFolder, uniqueFileName);

                //string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //Directory.CreateDirectory(Path.GetDirectoryName(filePath));


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }
            return uploadsFolder + "/" + uniqueFileName;

        }


        public async Task<string> UploadIFormPdfAsync(IFormFile file)
        {
            string uploadsFolder = string.Empty;
            string uniqueFileName = string.Empty;

            if (file != null && file.Length > 0)
            {
                if (file.Length > 60 * 1024 * 1024) // 60MB in bytes
                {
                    throw new ArgumentException("File size exceeds the limit of 60MB.");
                }

                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                // Ensure the file is a PDF
                if (fileExtension != ".pdf")
                {
                    throw new ArgumentException("Only PDF files are allowed.");
                }

                uploadsFolder = "pdfuploads"; // You can customize this

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                string directoryPath = Path.Combine(_env.WebRootPath, "Attachments", uploadsFolder);
                Directory.CreateDirectory(directoryPath); // Ensure directory exists

                string filePath = Path.Combine(directoryPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Path.Combine(uploadsFolder, uniqueFileName).Replace("\\", "/");
            }

            throw new ArgumentException("Invalid file.");
        }

    }
}
