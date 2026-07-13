namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos
{
    public class CreateTeacherDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Branch { get; set; }
    }   
}
