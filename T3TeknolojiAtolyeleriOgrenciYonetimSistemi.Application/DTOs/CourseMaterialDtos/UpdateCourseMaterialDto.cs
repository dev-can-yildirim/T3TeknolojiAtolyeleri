namespace T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos
{
    public class UpdateCourseMaterialDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
