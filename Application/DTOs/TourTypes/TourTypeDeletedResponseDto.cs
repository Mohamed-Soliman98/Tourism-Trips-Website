namespace Application.DTOs.TourTypes
{
    public sealed record TourTypeDeletedResponseDto(
        string Message = "Tour type deleted successfully."
    );
}
