namespace Domain.Entitys
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public string? DeviceName { get; set; }

        public string? Browser { get; set; }

        public string? OperatingSystem { get; set; }

        public string? IpAddress { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked => RevokedAt.HasValue;

        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
