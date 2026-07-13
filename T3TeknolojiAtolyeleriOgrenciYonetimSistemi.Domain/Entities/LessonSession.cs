using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class LessonSession : BaseEntity
    {
        public int TeacherCourseAssignmentId { get; set; }

        public DateTime LessonDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        [MaxLength(300)]
        public string? Topic { get; set; }

        public bool IsCompleted { get; set; } = false;

        // İlişki
        public TeacherCourseAssignment TeacherCourseAssignment { get; set; } = null!;

        // Bu ders oturumunun yoklama kayıtları
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        // Bu ders oturumuyla ilgili notlar
        public ICollection<TeacherNote> TeacherNotes { get; set; } = new List<TeacherNote>();
    }
}
