using System.ComponentModel.DataAnnotations;
using T3TeknolojiAtolyeleri.Domain.Common;
using T3TeknolojiAtolyeleri.Domain.Enums;

namespace T3TeknolojiAtolyeleri.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        // Bir kullanıcı öğretmen hesabıysa Teacher tablosuyla bağlantılı olur.
        public Teacher? Teacher { get; set; }
    }
}
