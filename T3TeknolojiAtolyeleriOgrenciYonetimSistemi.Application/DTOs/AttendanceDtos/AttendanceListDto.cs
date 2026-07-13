using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos
{
    public class AttendanceListDto
    {
        public int Id { get; set; }

        public int LessonSessionId { get; set; }

        public int StudentId { get; set; }

        public string StudentFullName { get; set; } = null!;

        public string StudentEmail { get; set; } = null!;

        public string StudentPhoneNumber { get; set; } = null!;

        public AttendanceStatus Status { get; set; }

        public string? Description { get; set; }

        public DateTime RecordedAt { get; set; }

        public int TotalAbsenceDays { get; set; }

        public bool RequiresWarning { get; set; }
    }
}
