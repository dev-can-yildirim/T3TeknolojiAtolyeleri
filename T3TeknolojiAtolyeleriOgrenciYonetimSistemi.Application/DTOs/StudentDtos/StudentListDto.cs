using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.StudentDtos
{
    public class StudentListDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public EducationLevel EducationLevel { get; set; }

        public int GradeLevel { get; set; }

        public bool HasPurchasedEducationSet { get; set; }

        public bool IsActive { get; set; }
    }
}
