using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class LessonSessionService : ILessonSessionService
    {
        private readonly IGenericRepository<LessonSession> _lessonSessionRepository;
        private readonly IGenericRepository<TeacherCourseAssignment> _assignmentRepository;

        private static readonly string[] IncludePaths =
        {
            "TeacherCourseAssignment.Teacher",
            "TeacherCourseAssignment.Course",
            "TeacherCourseAssignment.ClassGroup"
        };

        public LessonSessionService(
            IGenericRepository<LessonSession> lessonSessionRepository,
            IGenericRepository<TeacherCourseAssignment> assignmentRepository)
        {
            _lessonSessionRepository = lessonSessionRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<ServiceResponse<List<LessonSessionListDto>>> GetAllAsync()
        {
            var sessions = await _lessonSessionRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = sessions
                .OrderByDescending(x => x.LessonDate)
                .ThenBy(x => x.StartTime)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<LessonSessionListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<LessonSessionListDto>> GetByIdAsync(int id)
        {
            var session = await _lessonSessionRepository.GetByIdWithIncludePathsAsync(id, IncludePaths);

            if (session == null)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Ders oturumu bulunamadı.");
            }

            return ServiceResponse<LessonSessionListDto>.Ok(MapToListDto(session));
        }

        public async Task<ServiceResponse<List<LessonSessionListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(teacherCourseAssignmentId);

            if (assignment == null)
            {
                return ServiceResponse<List<LessonSessionListDto>>.Fail("Öğretmen ders ataması bulunamadı.");
            }

            var sessions = await _lessonSessionRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = sessions
                .Where(x => x.TeacherCourseAssignmentId == teacherCourseAssignmentId)
                .OrderByDescending(x => x.LessonDate)
                .ThenBy(x => x.StartTime)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<LessonSessionListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<LessonSessionListDto>> CreateAsync(CreateLessonSessionDto dto)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(dto.TeacherCourseAssignmentId);

            if (assignment == null || !assignment.IsActive)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Aktif öğretmen ders ataması bulunamadı.");
            }

            var lessonDate = dto.LessonDate.Date;
            var dayStart = lessonDate;
            var dayEnd = lessonDate.AddDays(1);

            var sessionExists = await _lessonSessionRepository.AnyAsync(x =>
                x.TeacherCourseAssignmentId == dto.TeacherCourseAssignmentId &&
                x.LessonDate >= dayStart &&
                x.LessonDate < dayEnd &&
                x.StartTime == dto.StartTime);

            if (sessionExists)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Bu atama için aynı tarih ve başlangıç saatinde ders oturumu zaten mevcut.");
            }

            var session = new LessonSession
            {
                TeacherCourseAssignmentId = dto.TeacherCourseAssignmentId,
                LessonDate = lessonDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Topic = dto.Topic?.Trim(),
                IsCompleted = false
            };

            await _lessonSessionRepository.AddAsync(session);
            await _lessonSessionRepository.SaveChangesAsync();

            var createdSession = await _lessonSessionRepository.GetByIdWithIncludePathsAsync(session.Id, IncludePaths);

            return ServiceResponse<LessonSessionListDto>.Ok(
                MapToListDto(createdSession!),
                "Ders oturumu başarıyla oluşturuldu."
            );
        }

        public async Task<ServiceResponse<LessonSessionListDto>> UpdateAsync(UpdateLessonSessionDto dto)
        {
            var session = await _lessonSessionRepository.GetByIdAsync(dto.Id);

            if (session == null)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Güncellenecek ders oturumu bulunamadı.");
            }

            var assignment = await _assignmentRepository.GetByIdAsync(dto.TeacherCourseAssignmentId);

            if (assignment == null || !assignment.IsActive)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Aktif öğretmen ders ataması bulunamadı.");
            }

            var lessonDate = dto.LessonDate.Date;
            var dayStart = lessonDate;
            var dayEnd = lessonDate.AddDays(1);

            var sessionExists = await _lessonSessionRepository.AnyAsync(x =>
                x.TeacherCourseAssignmentId == dto.TeacherCourseAssignmentId &&
                x.LessonDate >= dayStart &&
                x.LessonDate < dayEnd &&
                x.StartTime == dto.StartTime &&
                x.Id != dto.Id);

            if (sessionExists)
            {
                return ServiceResponse<LessonSessionListDto>.Fail("Bu atama için aynı tarih ve başlangıç saatinde başka bir ders oturumu zaten mevcut.");
            }

            session.TeacherCourseAssignmentId = dto.TeacherCourseAssignmentId;
            session.LessonDate = lessonDate;
            session.StartTime = dto.StartTime;
            session.EndTime = dto.EndTime;
            session.Topic = dto.Topic?.Trim();
            session.IsCompleted = dto.IsCompleted;

            _lessonSessionRepository.Update(session);
            await _lessonSessionRepository.SaveChangesAsync();

            var updatedSession = await _lessonSessionRepository.GetByIdWithIncludePathsAsync(session.Id, IncludePaths);

            return ServiceResponse<LessonSessionListDto>.Ok(
                MapToListDto(updatedSession!),
                "Ders oturumu başarıyla güncellendi."
            );
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var session = await _lessonSessionRepository.GetByIdAsync(id);

            if (session == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek ders oturumu bulunamadı.");
            }

            _lessonSessionRepository.SoftDelete(session);
            await _lessonSessionRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Ders oturumu başarıyla silindi.");
        }

        private static LessonSessionListDto MapToListDto(LessonSession session)
        {
            var assignment = session.TeacherCourseAssignment;

            return new LessonSessionListDto
            {
                Id = session.Id,
                TeacherCourseAssignmentId = session.TeacherCourseAssignmentId,

                TeacherId = assignment.TeacherId,
                TeacherFullName = $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",

                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,

                ClassGroupId = assignment.ClassGroupId,
                ClassGroupName = assignment.ClassGroup.Name,

                LessonDate = session.LessonDate,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Topic = session.Topic,
                IsCompleted = session.IsCompleted
            };
        }
    }
}
