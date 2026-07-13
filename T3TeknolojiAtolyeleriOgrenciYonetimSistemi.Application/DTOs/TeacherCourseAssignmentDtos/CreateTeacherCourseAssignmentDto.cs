namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos
{
    public class CreateTeacherCourseAssignmentDto
    {
        public int TeacherId { get; set; }

        public int CourseId { get; set; }

        public int ClassGroupId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
