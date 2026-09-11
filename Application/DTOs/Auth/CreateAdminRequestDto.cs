namespace Application.DTOs.Auth
{
    public sealed record CreateAdminRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
