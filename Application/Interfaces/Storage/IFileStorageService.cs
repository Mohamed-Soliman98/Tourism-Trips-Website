using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Storage
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(IFormFile file, string folderName, CancellationToken cancellationToken = default);
        Task DeleteAsync(string relativeOrPublicUrl);
    }
}
