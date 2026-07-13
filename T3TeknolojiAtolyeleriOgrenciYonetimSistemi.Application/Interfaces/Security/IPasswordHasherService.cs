namespace T3TeknolojiAtolyeleriApplication.Interfaces.Security
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);
    }
}
