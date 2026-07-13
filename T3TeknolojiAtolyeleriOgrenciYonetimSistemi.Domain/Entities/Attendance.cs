using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public int LessonSessionId { get; set; }

        public int StudentId { get; set; }

        public AttendanceStatus Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.Now;

        // İlişkiler
        public LessonSession LessonSession { get; set; } = null!;

        public Student Student { get; set; } = null!;
    }
}
