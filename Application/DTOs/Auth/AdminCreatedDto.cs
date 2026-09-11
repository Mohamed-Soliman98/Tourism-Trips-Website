namespace Application.DTOs.Auth
{
    public sealed record AdminCreatedDto(
        Guid Id,
        string FullName,
        string Email,
        string Role);
}
