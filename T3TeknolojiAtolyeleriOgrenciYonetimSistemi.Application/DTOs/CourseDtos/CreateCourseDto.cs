using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos
{
    public class CreateCourseDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public EducationLevel EducationLevel { get; set; }
    }
}
