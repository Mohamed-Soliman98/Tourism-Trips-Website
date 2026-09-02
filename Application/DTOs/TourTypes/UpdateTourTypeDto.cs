namespace Application.DTOs.TourTypes
{
    public sealed record UpdateTourTypeDto(
        string Name,
        bool IsActive
    );
}
