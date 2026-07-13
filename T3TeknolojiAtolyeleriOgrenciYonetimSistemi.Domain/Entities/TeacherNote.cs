using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class TeacherNote : BaseEntity
    {
        public int TeacherId { get; set; }

        public int TeacherCourseAssignmentId { get; set; }

        public int? LessonSessionId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = null!;

        public DateTime NoteDate { get; set; } = DateTime.Now;

        // İlişkiler
        public Teacher Teacher { get; set; } = null!;

        public TeacherCourseAssignment TeacherCourseAssignment { get; set; } = null!;

        public LessonSession? LessonSession { get; set; }
    }
}
