namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos
{
    public class TeacherNoteListDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public string TeacherFullName { get; set; } = null!;

        public int TeacherCourseAssignmentId { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int ClassGroupId { get; set; }

        public string ClassGroupName { get; set; } = null!;

        public int? LessonSessionId { get; set; }

        public DateTime? LessonDate { get; set; }

        public string Content { get; set; } = null!;

        public DateTime NoteDate { get; set; }
    }
}
