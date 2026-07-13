namespace T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos
{
    public class StudentClassGroupListDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string StudentFullName { get; set; } = null!;

        public string StudentEmail { get; set; } = null!;

        public string StudentPhoneNumber { get; set; } = null!;

        public int ClassGroupId { get; set; }

        public string ClassGroupName { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; }
    }
}
