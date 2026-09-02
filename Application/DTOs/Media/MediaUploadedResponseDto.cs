namespace Application.DTOs.Media
{
    public sealed record MediaUploadedResponseDto(
        string Url,
        string FileName
    );
}
