using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class ClassGroup : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public EducationLevel EducationLevel { get; set; }

        public int GradeLevel { get; set; }

        [MaxLength(20)]
        public string? AcademicYear { get; set; }

        public bool IsActive { get; set; } = true;

        // Bu sınıfa/gruba kayıtlı öğrenciler
        public ICollection<StudentClassGroup> StudentClassGroups { get; set; } = new List<StudentClassGroup>();

        // Bu sınıfa/gruba atanmış ders-öğretmen kayıtları
        public ICollection<TeacherCourseAssignment> TeacherCourseAssignments { get; set; } = new List<TeacherCourseAssignment>();
    }
}
