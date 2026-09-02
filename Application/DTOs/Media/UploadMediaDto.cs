using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Media
{
    public sealed record UploadMediaDto(IFormFile File);
}
