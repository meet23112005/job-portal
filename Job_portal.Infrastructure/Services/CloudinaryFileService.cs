using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Job_portal.Infrastructure.Services
{
    public class CloudinaryFileService : IFileService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryFileService> _logger;

        public CloudinaryFileService(IOptions<CloudinarySettings> cloudinarySettings, ILogger<CloudinaryFileService> logger)
        {
            _logger = logger;

            var settings = cloudinarySettings.Value;
            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;//always use https
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string folder)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = $"job_portal/{folder}",
                // resize image → saves storage + faster loading
                Transformation = new Transformation()
                    .Width(500)
                    .Height(500)
                    .Crop("fill")
                    .Quality("auto")   // auto optimize quality
                    .FetchFormat("auto") // auto best format (webp etc)
            };
            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
            {
                _logger.LogError("Cloudinary upload failed: {Error}",
                    result.Error.Message);
                throw new Exception($"Image upload failed: {result.Error.Message}");
            }

            _logger.LogInformation("Image uploaded to Cloudinary: {Url}",
                result.SecureUrl);

            return result.SecureUrl.ToString();
            // returns → "https://res.cloudinary.com/cloud/image/upload/job_portal/profiles/photo.jpg"

        }

        public async Task<string> UploadResumeAsync(Stream fileStream, string fileName, string originalName)
        {
            var uploadPatrams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "job_portal/resumes"
            };

            var result = await _cloudinary.UploadAsync(uploadPatrams);
            if (result.Error != null)
            {
                _logger.LogError("Cloudinary resume upload failed: {Error}",
                   result.Error.Message);
                throw new Exception($"Resume upload failed: {result.Error.Message}");
            }

            _logger.LogInformation("Resume uploaded: {OriginalName}", originalName);

            return result.SecureUrl.ToString();
        }
        public async Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                var publicId = ExtractPublicId(fileUrl);

                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Result == "ok")
                    _logger.LogInformation("File deleted from Cloudinary: {PublicId}", publicId);
                else
                    _logger.LogWarning("Cloudinary delete returned: {Result}", result.Result);
            }
            catch (Exception ex)
            {
                // log but don't throw — deletion failure
                // should not break the update operation
                _logger.LogWarning("Failed to delete file {FileUrl}: {Error}",
                    fileUrl, ex.Message);
            }
        }
        private string ExtractPublicId(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var segments = uri.AbsolutePath.Split('/');

            // find "upload" segment index
            var uploadIndex = Array.IndexOf(segments, "upload");

            // skip "upload" and version "v123" segments
            var relevantSegments = segments
                .Skip(uploadIndex + 2)  // skip "upload" + "v123"
                .ToArray();

            // join and remove file extension
            var publicId = string.Join("/", relevantSegments);
            var dotIndex = publicId.LastIndexOf('.');

            return dotIndex > 0
                ? publicId[..dotIndex]  // remove extension
                : publicId;
        }
    }
}
