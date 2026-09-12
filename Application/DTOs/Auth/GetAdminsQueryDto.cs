namespace Application.DTOs.Auth
{
    public sealed record GetAdminsQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
