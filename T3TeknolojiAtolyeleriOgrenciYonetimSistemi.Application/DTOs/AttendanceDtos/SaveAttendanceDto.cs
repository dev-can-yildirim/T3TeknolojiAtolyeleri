namespace T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos
{
    public class SaveAttendanceDto
    {
        public int LessonSessionId { get; set; }

        public List<SaveAttendanceItemDto> Items { get; set; } = new();
    }
}
