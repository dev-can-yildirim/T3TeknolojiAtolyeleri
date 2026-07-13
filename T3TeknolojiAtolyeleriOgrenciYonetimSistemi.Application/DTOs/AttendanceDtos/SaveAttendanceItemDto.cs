using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos
{
    public class SaveAttendanceItemDto
    {
        public int StudentId { get; set; }

        public AttendanceStatus Status { get; set; }

        public string? Description { get; set; }
    }
}
