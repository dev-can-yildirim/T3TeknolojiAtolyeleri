using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Auth
{
    public interface IJwtTokenService
    {
        JwtTokenDto GenerateToken(AppUser user, int? teacherId);
    }
}
