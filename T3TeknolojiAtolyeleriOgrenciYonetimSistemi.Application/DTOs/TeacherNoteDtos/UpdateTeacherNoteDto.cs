namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos
{
    public class UpdateTeacherNoteDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public int TeacherCourseAssignmentId { get; set; }

        public int? LessonSessionId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime NoteDate { get; set; }
    }
}
