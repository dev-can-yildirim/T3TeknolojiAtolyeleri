namespace T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }

        public int? TeacherId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
