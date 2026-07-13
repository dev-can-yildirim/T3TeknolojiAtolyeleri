using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos
{
    public class UpdateTeacherCourseAssignmentDto
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public int CourseId { get; set; }

        public int ClassGroupId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
