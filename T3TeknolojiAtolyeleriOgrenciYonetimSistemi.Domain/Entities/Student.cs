using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class Student : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        public EducationLevel EducationLevel { get; set; }

        public int GradeLevel { get; set; }

        public bool HasPurchasedEducationSet { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // Öğrencinin dahil olduğu sınıf/gruplar
        public ICollection<StudentClassGroup> StudentClassGroups { get; set; } = new List<StudentClassGroup>();

        // Öğrencinin yoklama kayıtları
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
