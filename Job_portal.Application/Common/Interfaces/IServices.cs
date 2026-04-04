using Job_portal.Domain.Entities;

namespace Job_portal.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public interface IJwtServiceAdmin
    {
        string GenerateAdminToken(string email);
    }

    public interface IFileService
    {
        Task<string> UploadImageAsync(Stream fileStream,string fileName,string folder);
        Task<string> UploadResumeAsync(Stream fileStream,string fileName,string originalName);
        Task DeleteFileAsync(string fileUrl);
    }

}
