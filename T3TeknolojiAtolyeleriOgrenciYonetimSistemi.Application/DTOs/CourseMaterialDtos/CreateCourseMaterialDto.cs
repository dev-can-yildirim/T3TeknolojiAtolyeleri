using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos
{
    public class CreateCourseMaterialDto
    {
        public int TeacherCourseAssignmentId { get; set; }

        public int UploadedByTeacherId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string FilePath { get; set; } = null!;

        public string OriginalFileName { get; set; } = null!;

        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public MaterialType MaterialType { get; set; }
    }
}
