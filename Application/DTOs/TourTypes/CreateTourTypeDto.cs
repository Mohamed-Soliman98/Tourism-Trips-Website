namespace Application.DTOs.TourTypes
{
    public sealed record CreateTourTypeDto(
        string Name,
        bool IsActive = true
    );
}
