using Job_portal.Application.Common.Interfaces;
using Job_portal.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Job_portal.Infrastructure.Services
{
    public class LocalFileService : IFileService
    {
        private readonly ILogger<LocalFileService> _logger;
        private readonly IWebHostEnvironment _env;

        public LocalFileService(ILogger<LocalFileService> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _env = environment;
        }


        //Profile Photo and CompanyLogo
        //returns: relative URL → "/uploads/profiles/filename.jpg" 
        //frontend uses this URL to display image
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(fileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file extension attempted: {Extension}", extension);
                throw new BadRequestException("Only JPG PNG WEBP images allowed");
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            var folderPath = Path.Combine(
                _env.WebRootPath, "uploads", folder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, uniqueFileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            _logger.LogInformation("Image Uploaded : {FilePath}", filePath);

            return $"/uploads/{folder}/{uniqueFileName}";
        }

        public async Task<string> UploadResumeAsync(Stream fileStream, string fileName, string originalName)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var folderPath = Path.Combine(_env.WebRootPath, "uploads", "resumes");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, uniqueFileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);

            _logger.LogInformation("Resume uploaded: {OriginalName}", originalName);

            return $"/uploads/resumes/{uniqueFileName}";
        }

        public Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                // convert URL to physical path
                // "/uploads/profiles/file.jpg"
                // → "wwwroot/uploads/profiles/file.jpg"
                var filePath = Path.Combine(
                    _env.WebRootPath
                        ,fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("File deleted: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to delete file {FileUrl}: {Error}",
                        fileUrl, ex.Message);
            }
            return Task.CompletedTask;
        }


    }
}
