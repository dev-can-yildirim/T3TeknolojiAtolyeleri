namespace T3TeknolojiAtolyeleri.API.Requests
{

    public class UploadCourseMaterialRequest
    {
        public int TeacherCourseAssignmentId { get; set; }

        public int UploadedByTeacherId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile File { get; set; } = null!;
    }

}
