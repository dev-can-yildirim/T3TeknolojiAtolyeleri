using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class Course : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public EducationLevel EducationLevel { get; set; }

        public bool IsActive { get; set; } = true;

        // Bu dersin hangi öğretmen/sınıf atamalarında kullanıldığı
        public ICollection<TeacherCourseAssignment> TeacherCourseAssignments { get; set; } = new List<TeacherCourseAssignment>();
    }
}
