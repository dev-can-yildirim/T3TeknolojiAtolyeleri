namespace T3TeknolojiAtolyeleriApplication.Interfaces.Auth
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }

        int? UserId { get; }

        int? TeacherId { get; }

        string? Role { get; }

        bool IsAdmin { get; }

        bool IsTeacher { get; }
    }
}
