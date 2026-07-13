namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos
{
    public class TeacherCourseAssignmentListDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public string TeacherFullName { get; set; } = null!;

        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public int ClassGroupId { get; set; }

        public string ClassGroupName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
