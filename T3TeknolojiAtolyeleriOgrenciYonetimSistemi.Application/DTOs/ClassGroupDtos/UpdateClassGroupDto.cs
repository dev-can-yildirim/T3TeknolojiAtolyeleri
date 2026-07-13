using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.ClassGroupDtos
{
    public class UpdateClassGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public EducationLevel EducationLevel { get; set; }

        public int GradeLevel { get; set; }

        public string? AcademicYear { get; set; }

        public bool IsActive { get; set; }
    }
}
