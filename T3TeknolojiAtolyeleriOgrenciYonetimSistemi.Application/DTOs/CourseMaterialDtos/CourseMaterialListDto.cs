using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos
{
    public class CourseMaterialListDto
    {
        public int Id { get; set; }

        public int TeacherCourseAssignmentId { get; set; }

        public int TeacherId { get; set; }

        public string TeacherFullName { get; set; } = null!;

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int ClassGroupId { get; set; }

        public string ClassGroupName { get; set; } = null!;

        public int UploadedByTeacherId { get; set; }

        public string UploadedByTeacherFullName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string FilePath { get; set; } = null!;

        public string OriginalFileName { get; set; } = null!;

        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public MaterialType MaterialType { get; set; }

        public DateTime UploadedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
