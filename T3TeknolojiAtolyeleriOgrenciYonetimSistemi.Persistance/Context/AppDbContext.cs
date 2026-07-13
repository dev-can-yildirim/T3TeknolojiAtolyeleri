using Microsoft.EntityFrameworkCore;
using T3TeknolojiAtolyeleri.Domain.Entities;

namespace T3TeknolojiAtolyeleri.Persistance.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassGroup> ClassGroups { get; set; }
        public DbSet<StudentClassGroup> StudentClassGroups { get; set; }
        public DbSet<TeacherCourseAssignment> TeacherCourseAssignments { get; set; }
        public DbSet<LessonSession> LessonSessions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<TeacherNote> TeacherNotes { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * AppUser
             * Sisteme giriş yapan kullanıcılar: Admin ve Teacher.
             */
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUsers");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .HasConversion<int>();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * Teacher
             * Öğretmen detay tablosu.
             * Her Teacher bir AppUser hesabına bağlıdır.
             */
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.PhoneNumber)
                    .HasMaxLength(20);

                entity.Property(x => x.Branch)
                    .HasMaxLength(100);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.AppUser)
                    .WithOne(x => x.Teacher)
                    .HasForeignKey<Teacher>(x => x.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * Student
             * Öğrenci tablosu.
             * Öğrenciler sisteme giriş yapmaz.
             */
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(x => x.EducationLevel)
                    .HasConversion<int>();

                entity.Property(x => x.HasPurchasedEducationSet)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * Course
             * Ders tablosu.
             * Örnek: Matematik, Türkçe, Fen Bilimleri.
             */
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.EducationLevel)
                    .HasConversion<int>();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * ClassGroup
             * Sınıf veya kurs grubu tablosu.
             * Örnek: 8-A, TYT Hafta Sonu Grubu.
             */
            modelBuilder.Entity<ClassGroup>(entity =>
            {
                entity.ToTable("ClassGroups");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.EducationLevel)
                    .HasConversion<int>();

                entity.Property(x => x.AcademicYear)
                    .HasMaxLength(20);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * StudentClassGroup
             * Öğrenci - sınıf/grup ara tablosu.
             * Many-to-Many ilişkiyi temsil eder.
             */
            modelBuilder.Entity<StudentClassGroup>(entity =>
            {
                entity.ToTable("StudentClassGroups");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.Student)
                    .WithMany(x => x.StudentClassGroups)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ClassGroup)
                    .WithMany(x => x.StudentClassGroups)
                    .HasForeignKey(x => x.ClassGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.StudentId, x.ClassGroupId })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * TeacherCourseAssignment
             * Öğretmen - Ders - Sınıf/Grup ataması.
             * Sistemin ana ilişki tablolarından biridir.
             */
            modelBuilder.Entity<TeacherCourseAssignment>(entity =>
            {
                entity.ToTable("TeacherCourseAssignments");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.Teacher)
                    .WithMany(x => x.TeacherCourseAssignments)
                    .HasForeignKey(x => x.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Course)
                    .WithMany(x => x.TeacherCourseAssignments)
                    .HasForeignKey(x => x.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ClassGroup)
                    .WithMany(x => x.TeacherCourseAssignments)
                    .HasForeignKey(x => x.ClassGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.TeacherId, x.CourseId, x.ClassGroupId })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0 AND [IsActive] = 1");

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * LessonSession
             * İşlenen ders oturumu.
             * Örnek: 8-A Matematik, 01.07.2026, 10:00-11:30.
             */
            modelBuilder.Entity<LessonSession>(entity =>
            {
                entity.ToTable("LessonSessions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Topic)
                    .HasMaxLength(300);

                entity.Property(x => x.IsCompleted)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.TeacherCourseAssignment)
                    .WithMany(x => x.LessonSessions)
                    .HasForeignKey(x => x.TeacherCourseAssignmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.TeacherCourseAssignmentId, x.LessonDate });

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * Attendance
             * Yoklama kaydı.
             * Aynı öğrenci için aynı ders oturumunda sadece bir yoklama kaydı olmalı.
             */
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendances");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Status)
                    .HasConversion<int>();

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.LessonSession)
                    .WithMany(x => x.Attendances)
                    .HasForeignKey(x => x.LessonSessionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Student)
                    .WithMany(x => x.Attendances)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => new { x.LessonSessionId, x.StudentId })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * TeacherNote
             * Öğretmenin özel notları.
             * Sadece ilgili öğretmen görecek şekilde API tarafında filtrelenecek.
             */
            modelBuilder.Entity<TeacherNote>(entity =>
            {
                entity.ToTable("TeacherNotes");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Content)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.Teacher)
                    .WithMany(x => x.TeacherNotes)
                    .HasForeignKey(x => x.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.TeacherCourseAssignment)
                    .WithMany(x => x.TeacherNotes)
                    .HasForeignKey(x => x.TeacherCourseAssignmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.LessonSession)
                    .WithMany(x => x.TeacherNotes)
                    .HasForeignKey(x => x.LessonSessionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });

            /*
             * CourseMaterial
             * PDF, Word, sunum gibi materyal dosyalarının bilgileri.
             * Dosyanın kendisi veritabanında tutulmayacak.
             */
            modelBuilder.Entity<CourseMaterial>(entity =>
            {
                entity.ToTable("CourseMaterials");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .HasMaxLength(500);

                entity.Property(x => x.FilePath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(x => x.OriginalFileName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.ContentType)
                    .HasMaxLength(100);

                entity.Property(x => x.MaterialType)
                    .HasConversion<int>();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.TeacherCourseAssignment)
                    .WithMany(x => x.CourseMaterials)
                    .HasForeignKey(x => x.TeacherCourseAssignmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.UploadedByTeacher)
                    .WithMany(x => x.UploadedMaterials)
                    .HasForeignKey(x => x.UploadedByTeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(x => !x.IsDeleted);
            });
        }
    }
}