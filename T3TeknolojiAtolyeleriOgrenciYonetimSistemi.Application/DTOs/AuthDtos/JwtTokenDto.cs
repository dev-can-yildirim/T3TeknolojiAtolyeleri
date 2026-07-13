namespace T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos
{
    public class JwtTokenDto
    {
        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
