using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public int AppUserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Branch { get; set; }

        public bool IsActive { get; set; } = true;

        // AppUser ilişkisi
        public AppUser AppUser { get; set; } = null!;

        // Öğretmenin ders/sınıf atamaları
        public ICollection<TeacherCourseAssignment> TeacherCourseAssignments { get; set; } = new List<TeacherCourseAssignment>();

        // Öğretmenin özel notları
        public ICollection<TeacherNote> TeacherNotes { get; set; } = new List<TeacherNote>();

        // Öğretmenin yüklediği materyaller
        public ICollection<CourseMaterial> UploadedMaterials { get; set; } = new List<CourseMaterial>();
    }
}
