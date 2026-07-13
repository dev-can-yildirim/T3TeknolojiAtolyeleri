using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class CourseMaterial : BaseEntity
    {
        public int TeacherCourseAssignmentId { get; set; }

        public int UploadedByTeacherId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string OriginalFileName { get; set; } = null!;

        [MaxLength(100)]
        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public MaterialType MaterialType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // İlişkiler
        public TeacherCourseAssignment TeacherCourseAssignment { get; set; } = null!;

        public Teacher UploadedByTeacher { get; set; } = null!;
    }
}
