namespace T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos
{
    public class AttendanceWarningDto
    {
        public int StudentId { get; set; }

        public string StudentFullName { get; set; } = null!;

        public string StudentEmail { get; set; } = null!;

        public string StudentPhoneNumber { get; set; } = null!;

        public int TotalAbsenceDays { get; set; }

        public bool RequiresWarning { get; set; }
    }
}
