using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int? UserId
        {
            get
            {
                var userIdValue =
                    _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value
                    ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return int.TryParse(userIdValue, out var userId) ? userId : null;
            }
        }

        public int? TeacherId
        {
            get
            {
                var teacherIdValue =
                    _httpContextAccessor.HttpContext?.User?.FindFirst("TeacherId")?.Value;

                return int.TryParse(teacherIdValue, out var teacherId) ? teacherId : null;
            }
        }

        public string? Role =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("Role")?.Value
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public bool IsAdmin => Role == "Admin";

        public bool IsTeacher => Role == "Teacher";
    }
}
