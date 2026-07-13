namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos
{
    public class UpdateTeacherDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Branch { get; set; }

        public bool IsActive { get; set; }
    }
}
