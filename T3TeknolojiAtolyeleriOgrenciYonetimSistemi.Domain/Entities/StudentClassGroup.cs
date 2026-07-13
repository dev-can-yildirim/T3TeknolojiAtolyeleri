using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Common;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class StudentClassGroup : BaseEntity
    {
        public int StudentId { get; set; }

        public int ClassGroupId { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // İlişkiler
        public Student Student { get; set; } = null!;

        public ClassGroup ClassGroup { get; set; } = null!;
    }
}
