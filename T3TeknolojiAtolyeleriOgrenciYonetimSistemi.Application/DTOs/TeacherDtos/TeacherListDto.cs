using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos
{
    public class TeacherListDto
    {
        public int Id { get; set; }

        public int AppUserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Branch { get; set; }

        public bool IsActive { get; set; }
    }
}
