namespace Application.DTOs.CMSSections
{
    public sealed record CMSSectionDeletedResponseDto(
        Guid Id,
        string Message
    );
}