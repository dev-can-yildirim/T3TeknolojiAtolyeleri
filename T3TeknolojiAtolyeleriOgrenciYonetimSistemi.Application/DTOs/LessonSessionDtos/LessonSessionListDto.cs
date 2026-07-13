namespace T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos
{
    public class LessonSessionListDto
    {
        public int Id { get; set; }

        public int TeacherCourseAssignmentId { get; set; }

        public int TeacherId { get; set; }

        public string TeacherFullName { get; set; } = null!;

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int ClassGroupId { get; set; }

        public string ClassGroupName { get; set; } = null!;

        public DateTime LessonDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string? Topic { get; set; }

        public bool IsCompleted { get; set; }
    }
}
