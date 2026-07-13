using T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
}
