namespace T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos
{
    public class UpdateLessonSessionDto
    {
        public int Id { get; set; }

        public int TeacherCourseAssignmentId { get; set; }

        public DateTime LessonDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string? Topic { get; set; }

        public bool IsCompleted { get; set; }
    }
}
