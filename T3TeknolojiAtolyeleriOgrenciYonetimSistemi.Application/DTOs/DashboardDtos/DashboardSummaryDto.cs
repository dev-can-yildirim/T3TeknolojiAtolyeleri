namespace T3TeknolojiAtolyeleriApplication.DTOs.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public string Scope { get; set; } = null!;

        public int? TeacherId { get; set; }

        public string? TeacherFullName { get; set; }

        public int TotalStudents { get; set; }

        public int ActiveStudents { get; set; }

        public int TotalTeachers { get; set; }

        public int ActiveTeachers { get; set; }

        public int TotalCourses { get; set; }

        public int ActiveCourses { get; set; }

        public int TotalClassGroups { get; set; }

        public int ActiveClassGroups { get; set; }

        public int TotalAssignments { get; set; }

        public int ActiveAssignments { get; set; }

        public int TotalLessonSessions { get; set; }

        public int CompletedLessonSessions { get; set; }

        public int TodayLessonSessions { get; set; }

        public int TotalMaterials { get; set; }

        public int ActiveMaterials { get; set; }

        public int TotalTeacherNotes { get; set; }

        public int WarningStudentCount { get; set; }

        public DateTime GeneratedAt { get; set; }
    }
}
