namespace T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos
{
    public class UpdateStudentClassGroupDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int ClassGroupId { get; set; }

        public bool IsActive { get; set; }
    }
}
