using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.DashboardDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Teacher> _teacherRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<ClassGroup> _classGroupRepository;
        private readonly IGenericRepository<TeacherCourseAssignment> _assignmentRepository;
        private readonly IGenericRepository<StudentClassGroup> _studentClassGroupRepository;
        private readonly IGenericRepository<LessonSession> _lessonSessionRepository;
        private readonly IGenericRepository<CourseMaterial> _courseMaterialRepository;
        private readonly IGenericRepository<TeacherNote> _teacherNoteRepository;
        private readonly IAttendanceService _attendanceService;

        public DashboardService(
            IGenericRepository<Student> studentRepository,
            IGenericRepository<Teacher> teacherRepository,
            IGenericRepository<Course> courseRepository,
            IGenericRepository<ClassGroup> classGroupRepository,
            IGenericRepository<TeacherCourseAssignment> assignmentRepository,
            IGenericRepository<StudentClassGroup> studentClassGroupRepository,
            IGenericRepository<LessonSession> lessonSessionRepository,
            IGenericRepository<CourseMaterial> courseMaterialRepository,
            IGenericRepository<TeacherNote> teacherNoteRepository,
            IAttendanceService attendanceService)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
            _classGroupRepository = classGroupRepository;
            _assignmentRepository = assignmentRepository;
            _studentClassGroupRepository = studentClassGroupRepository;
            _lessonSessionRepository = lessonSessionRepository;
            _courseMaterialRepository = courseMaterialRepository;
            _teacherNoteRepository = teacherNoteRepository;
            _attendanceService = attendanceService;
        }

        public async Task<ServiceResponse<DashboardSummaryDto>> GetAdminSummaryAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            var teachers = await _teacherRepository.GetAllAsync();
            var courses = await _courseRepository.GetAllAsync();
            var classGroups = await _classGroupRepository.GetAllAsync();
            var assignments = await _assignmentRepository.GetAllAsync();
            var lessonSessions = await _lessonSessionRepository.GetAllAsync();
            var materials = await _courseMaterialRepository.GetAllAsync();
            var teacherNotes = await _teacherNoteRepository.GetAllAsync();

            var warningResponse = await _attendanceService.GetWarningStudentsAsync();

            var today = DateTime.Today;

            var summary = new DashboardSummaryDto
            {
                Scope = "Admin",

                TotalStudents = students.Count,
                ActiveStudents = students.Count(x => x.IsActive),

                TotalTeachers = teachers.Count,
                ActiveTeachers = teachers.Count(x => x.IsActive),

                TotalCourses = courses.Count,
                ActiveCourses = courses.Count(x => x.IsActive),

                TotalClassGroups = classGroups.Count,
                ActiveClassGroups = classGroups.Count(x => x.IsActive),

                TotalAssignments = assignments.Count,
                ActiveAssignments = assignments.Count(x => x.IsActive),

                TotalLessonSessions = lessonSessions.Count,
                CompletedLessonSessions = lessonSessions.Count(x => x.IsCompleted),
                TodayLessonSessions = lessonSessions.Count(x => x.LessonDate.Date == today),

                TotalMaterials = materials.Count,
                ActiveMaterials = materials.Count(x => x.IsActive),

                TotalTeacherNotes = teacherNotes.Count,

                WarningStudentCount = warningResponse.Data?.Count ?? 0,

                GeneratedAt = DateTime.Now
            };

            return ServiceResponse<DashboardSummaryDto>.Ok(summary);
        }

        public async Task<ServiceResponse<DashboardSummaryDto>> GetTeacherSummaryAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<DashboardSummaryDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            var assignments = await _assignmentRepository.GetAllAsync();

            var teacherAssignments = assignments
                .Where(x => x.TeacherId == teacherId)
                .ToList();

            var activeTeacherAssignments = teacherAssignments
                .Where(x => x.IsActive)
                .ToList();

            var assignmentIds = teacherAssignments
                .Select(x => x.Id)
                .ToList();

            var classGroupIds = teacherAssignments
                .Select(x => x.ClassGroupId)
                .Distinct()
                .ToList();

            var courseIds = teacherAssignments
                .Select(x => x.CourseId)
                .Distinct()
                .ToList();

            var studentClassGroups = await _studentClassGroupRepository.GetAllWithIncludesAsync(
                x => x.Student
            );

            var teacherStudentGroups = studentClassGroups
                .Where(x =>
                    classGroupIds.Contains(x.ClassGroupId) &&
                    x.IsActive &&
                    x.Student.IsActive)
                .ToList();

            var totalStudents = teacherStudentGroups
                .Select(x => x.StudentId)
                .Distinct()
                .Count();

            var lessonSessions = await _lessonSessionRepository.GetAllAsync();

            var teacherLessonSessions = lessonSessions
                .Where(x => assignmentIds.Contains(x.TeacherCourseAssignmentId))
                .ToList();

            var materials = await _courseMaterialRepository.GetAllAsync();

            var teacherMaterials = materials
                .Where(x => x.UploadedByTeacherId == teacherId)
                .ToList();

            var teacherNotes = await _teacherNoteRepository.GetAllAsync();

            var teacherNoteCount = teacherNotes
                .Count(x => x.TeacherId == teacherId);

            var warningResponse = await _attendanceService.GetWarningStudentsByTeacherIdAsync(teacherId);

            var today = DateTime.Today;

            var summary = new DashboardSummaryDto
            {
                Scope = "Teacher",
                TeacherId = teacher.Id,
                TeacherFullName = $"{teacher.FirstName} {teacher.LastName}",

                TotalStudents = totalStudents,
                ActiveStudents = totalStudents,

                TotalTeachers = 1,
                ActiveTeachers = teacher.IsActive ? 1 : 0,

                TotalCourses = courseIds.Count,
                ActiveCourses = courseIds.Count,

                TotalClassGroups = classGroupIds.Count,
                ActiveClassGroups = classGroupIds.Count,

                TotalAssignments = teacherAssignments.Count,
                ActiveAssignments = activeTeacherAssignments.Count,

                TotalLessonSessions = teacherLessonSessions.Count,
                CompletedLessonSessions = teacherLessonSessions.Count(x => x.IsCompleted),
                TodayLessonSessions = teacherLessonSessions.Count(x => x.LessonDate.Date == today),

                TotalMaterials = teacherMaterials.Count,
                ActiveMaterials = teacherMaterials.Count(x => x.IsActive),

                TotalTeacherNotes = teacherNoteCount,

                WarningStudentCount = warningResponse.Data?.Count ?? 0,

                GeneratedAt = DateTime.Now
            };

            return ServiceResponse<DashboardSummaryDto>.Ok(summary);
        }
    }
}