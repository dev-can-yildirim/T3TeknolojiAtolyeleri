using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class TeacherCourseAssignment : BaseEntity
    {
        public int TeacherId { get; set; }

        public int CourseId { get; set; }

        public int ClassGroupId { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // İlişkiler
        public Teacher Teacher { get; set; } = null!;

        public Course Course { get; set; } = null!;

        public ClassGroup ClassGroup { get; set; } = null!;

        // Bu atamaya bağlı işlenen ders oturumları
        public ICollection<LessonSession> LessonSessions { get; set; } = new List<LessonSession>();

        // Bu atamaya bağlı öğretmen notları
        public ICollection<TeacherNote> TeacherNotes { get; set; } = new List<TeacherNote>();

        // Bu atamaya bağlı materyaller
        public ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
    }
}
