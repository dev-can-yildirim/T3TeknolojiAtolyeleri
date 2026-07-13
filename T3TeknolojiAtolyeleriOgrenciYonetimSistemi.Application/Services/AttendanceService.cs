using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IGenericRepository<Attendance> _attendanceRepository;
        private readonly IGenericRepository<LessonSession> _lessonSessionRepository;
        private readonly IGenericRepository<StudentClassGroup> _studentClassGroupRepository;

        private static readonly string[] LessonSessionIncludePaths =
        {
            "TeacherCourseAssignment",
            "TeacherCourseAssignment.ClassGroup",
            "TeacherCourseAssignment.Course",
            "TeacherCourseAssignment.Teacher"
        };

        public AttendanceService(
            IGenericRepository<Attendance> attendanceRepository,
            IGenericRepository<LessonSession> lessonSessionRepository,
            IGenericRepository<StudentClassGroup> studentClassGroupRepository)
        {
            _attendanceRepository = attendanceRepository;
            _lessonSessionRepository = lessonSessionRepository;
            _studentClassGroupRepository = studentClassGroupRepository;
        }

        public async Task<ServiceResponse<List<AttendanceListDto>>> GetByLessonSessionIdAsync(int lessonSessionId)
        {
            var lessonSession = await _lessonSessionRepository.GetByIdWithIncludePathsAsync(
                lessonSessionId,
                LessonSessionIncludePaths
            );

            if (lessonSession == null)
            {
                return ServiceResponse<List<AttendanceListDto>>.Fail("Ders oturumu bulunamadı.");
            }

            var attendances = await _attendanceRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.LessonSession
            );

            var allAttendancesWithLessons = await _attendanceRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.LessonSession
            );

            var absenceDayCounts = CalculateAbsenceDayCounts(allAttendancesWithLessons);

            var result = attendances
                .Where(x => x.LessonSessionId == lessonSessionId)
                .OrderBy(x => x.Student.FirstName)
                .ThenBy(x => x.Student.LastName)
                .Select(x => MapToListDto(x, absenceDayCounts))
                .ToList();

            return ServiceResponse<List<AttendanceListDto>>.Ok(result);
        }
        public async Task<ServiceResponse<List<AttendanceWarningDto>>> GetWarningStudentsByTeacherIdAsync(int teacherId)
        {
            var attendances = await _attendanceRepository.GetAllWithIncludePathsAsync(
                "Student",
                "LessonSession.TeacherCourseAssignment"
            );

            var result = attendances
                .Where(x =>
                    x.Status == AttendanceStatus.Absent &&
                    x.LessonSession.TeacherCourseAssignment.TeacherId == teacherId)
                .GroupBy(x => x.StudentId)
                .Select(group => new
                {
                    StudentId = group.Key,
                    Student = group.First().Student,
                    TotalAbsenceDays = group
                        .Select(x => x.LessonSession.LessonDate.Date)
                        .Distinct()
                        .Count()
                })
                .Where(x => x.TotalAbsenceDays >= 3)
                .OrderByDescending(x => x.TotalAbsenceDays)
                .Select(x => new AttendanceWarningDto
                {
                    StudentId = x.StudentId,
                    StudentFullName = $"{x.Student.FirstName} {x.Student.LastName}",
                    StudentEmail = x.Student.Email,
                    StudentPhoneNumber = x.Student.PhoneNumber,
                    TotalAbsenceDays = x.TotalAbsenceDays,
                    RequiresWarning = true
                })
                .ToList();

            return ServiceResponse<List<AttendanceWarningDto>>.Ok(result);
        }
        public async Task<ServiceResponse<List<AttendanceListDto>>> SaveAttendanceAsync(SaveAttendanceDto dto)
        {
            var lessonSession = await _lessonSessionRepository.GetByIdWithIncludePathsAsync(
                dto.LessonSessionId,
                LessonSessionIncludePaths
            );

            if (lessonSession == null)
            {
                return ServiceResponse<List<AttendanceListDto>>.Fail("Ders oturumu bulunamadı.");
            }

            var classGroupId = lessonSession.TeacherCourseAssignment.ClassGroupId;

            var classStudents = await _studentClassGroupRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.ClassGroup
            );

            var activeClassStudents = classStudents
                .Where(x => x.ClassGroupId == classGroupId && x.IsActive && x.Student.IsActive)
                .ToList();

            if (!activeClassStudents.Any())
            {
                return ServiceResponse<List<AttendanceListDto>>.Fail("Bu sınıf/grupta aktif öğrenci bulunamadı.");
            }

            var classStudentIds = activeClassStudents
                .Select(x => x.StudentId)
                .ToList();

            var incomingStudentIds = dto.Items
                .Select(x => x.StudentId)
                .ToList();

            var invalidStudentIds = incomingStudentIds
                .Where(studentId => !classStudentIds.Contains(studentId))
                .ToList();

            if (invalidStudentIds.Any())
            {
                return ServiceResponse<List<AttendanceListDto>>.Fail("Yoklama listesinde bu sınıf/gruba kayıtlı olmayan öğrenci var.");
            }

            var missingStudentIds = classStudentIds
                .Where(studentId => !incomingStudentIds.Contains(studentId))
                .ToList();

            if (missingStudentIds.Any())
            {
                return ServiceResponse<List<AttendanceListDto>>.Fail("Yoklama listesinde sınıftaki tüm öğrenciler bulunmalıdır.");
            }

            var existingAttendances = await _attendanceRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.LessonSession
            );

            var lessonAttendances = existingAttendances
                .Where(x => x.LessonSessionId == dto.LessonSessionId)
                .ToList();

            foreach (var item in dto.Items)
            {
                var existingAttendance = lessonAttendances
                    .FirstOrDefault(x => x.StudentId == item.StudentId);

                if (existingAttendance == null)
                {
                    var attendance = new Attendance
                    {
                        LessonSessionId = dto.LessonSessionId,
                        StudentId = item.StudentId,
                        Status = item.Status,
                        Description = item.Description?.Trim(),
                        RecordedAt = DateTime.Now
                    };

                    await _attendanceRepository.AddAsync(attendance);
                }
                else
                {
                    existingAttendance.Status = item.Status;
                    existingAttendance.Description = item.Description?.Trim();
                    existingAttendance.RecordedAt = DateTime.Now;

                    _attendanceRepository.Update(existingAttendance);
                }
            }

            await _attendanceRepository.SaveChangesAsync();

            lessonSession.IsCompleted = true;
            _lessonSessionRepository.Update(lessonSession);
            await _lessonSessionRepository.SaveChangesAsync();

            return await GetByLessonSessionIdAsync(dto.LessonSessionId);
        }

        public async Task<ServiceResponse<List<AttendanceWarningDto>>> GetWarningStudentsAsync()
        {
            var attendances = await _attendanceRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.LessonSession
            );

            var result = attendances
                .Where(x => x.Status == AttendanceStatus.Absent)
                .GroupBy(x => x.StudentId)
                .Select(group => new
                {
                    StudentId = group.Key,
                    Student = group.First().Student,
                    TotalAbsenceDays = group
                        .Select(x => x.LessonSession.LessonDate.Date)
                        .Distinct()
                        .Count()
                })
                .Where(x => x.TotalAbsenceDays >= 3)
                .OrderByDescending(x => x.TotalAbsenceDays)
                .Select(x => new AttendanceWarningDto
                {
                    StudentId = x.StudentId,
                    StudentFullName = $"{x.Student.FirstName} {x.Student.LastName}",
                    StudentEmail = x.Student.Email,
                    StudentPhoneNumber = x.Student.PhoneNumber,
                    TotalAbsenceDays = x.TotalAbsenceDays,
                    RequiresWarning = true
                })
                .ToList();

            return ServiceResponse<List<AttendanceWarningDto>>.Ok(result);
        }

        private static Dictionary<int, int> CalculateAbsenceDayCounts(List<Attendance> attendances)
        {
            return attendances
                .Where(x => x.Status == AttendanceStatus.Absent)
                .GroupBy(x => x.StudentId)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(x => x.LessonSession.LessonDate.Date)
                        .Distinct()
                        .Count()
                );
        }

        private static AttendanceListDto MapToListDto(
            Attendance attendance,
            Dictionary<int, int> absenceDayCounts)
        {
            var totalAbsenceDays = absenceDayCounts.ContainsKey(attendance.StudentId)
                ? absenceDayCounts[attendance.StudentId]
                : 0;

            return new AttendanceListDto
            {
                Id = attendance.Id,
                LessonSessionId = attendance.LessonSessionId,
                StudentId = attendance.StudentId,
                StudentFullName = $"{attendance.Student.FirstName} {attendance.Student.LastName}",
                StudentEmail = attendance.Student.Email,
                StudentPhoneNumber = attendance.Student.PhoneNumber,
                Status = attendance.Status,
                Description = attendance.Description,
                RecordedAt = attendance.RecordedAt,
                TotalAbsenceDays = totalAbsenceDays,
                RequiresWarning = totalAbsenceDays >= 3
            };
        }
    }
}
