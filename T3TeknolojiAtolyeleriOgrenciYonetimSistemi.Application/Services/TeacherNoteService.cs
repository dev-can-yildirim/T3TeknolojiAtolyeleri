using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class TeacherNoteService : ITeacherNoteService
    {
        private readonly IGenericRepository<TeacherNote> _teacherNoteRepository;
        private readonly IGenericRepository<Teacher> _teacherRepository;
        private readonly IGenericRepository<TeacherCourseAssignment> _assignmentRepository;
        private readonly IGenericRepository<LessonSession> _lessonSessionRepository;

        private static readonly string[] IncludePaths =
        {
            "Teacher",
            "TeacherCourseAssignment.Course",
            "TeacherCourseAssignment.ClassGroup",
            "LessonSession"
        };

        public TeacherNoteService(
            IGenericRepository<TeacherNote> teacherNoteRepository,
            IGenericRepository<Teacher> teacherRepository,
            IGenericRepository<TeacherCourseAssignment> assignmentRepository,
            IGenericRepository<LessonSession> lessonSessionRepository)
        {
            _teacherNoteRepository = teacherNoteRepository;
            _teacherRepository = teacherRepository;
            _assignmentRepository = assignmentRepository;
            _lessonSessionRepository = lessonSessionRepository;
        }

        public async Task<ServiceResponse<List<TeacherNoteListDto>>> GetAllAsync()
        {
            var notes = await _teacherNoteRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = notes
                .OrderByDescending(x => x.NoteDate)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<TeacherNoteListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherNoteListDto>> GetByIdAsync(int id)
        {
            var note = await _teacherNoteRepository.GetByIdWithIncludePathsAsync(id, IncludePaths);

            if (note == null)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Öğretmen notu bulunamadı.");
            }

            return ServiceResponse<TeacherNoteListDto>.Ok(MapToListDto(note));
        }

        public async Task<ServiceResponse<List<TeacherNoteListDto>>> GetByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                return ServiceResponse<List<TeacherNoteListDto>>.Fail("Öğretmen bulunamadı.");
            }

            var notes = await _teacherNoteRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = notes
                .Where(x => x.TeacherId == teacherId)
                .OrderByDescending(x => x.NoteDate)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<TeacherNoteListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<List<TeacherNoteListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(teacherCourseAssignmentId);

            if (assignment == null)
            {
                return ServiceResponse<List<TeacherNoteListDto>>.Fail("Öğretmen ders ataması bulunamadı.");
            }

            var notes = await _teacherNoteRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = notes
                .Where(x => x.TeacherCourseAssignmentId == teacherCourseAssignmentId)
                .OrderByDescending(x => x.NoteDate)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<TeacherNoteListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherNoteListDto>> CreateAsync(CreateTeacherNoteDto dto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            var assignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                dto.TeacherCourseAssignmentId,
                x => x.Course,
                x => x.ClassGroup
            );

            if (assignment == null || !assignment.IsActive)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Aktif öğretmen ders ataması bulunamadı.");
            }

            if (assignment.TeacherId != dto.TeacherId)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Bu ders ataması seçilen öğretmene ait değil.");
            }

            if (dto.LessonSessionId.HasValue)
            {
                var lessonSession = await _lessonSessionRepository.GetByIdAsync(dto.LessonSessionId.Value);

                if (lessonSession == null)
                {
                    return ServiceResponse<TeacherNoteListDto>.Fail("Ders oturumu bulunamadı.");
                }

                if (lessonSession.TeacherCourseAssignmentId != dto.TeacherCourseAssignmentId)
                {
                    return ServiceResponse<TeacherNoteListDto>.Fail("Seçilen ders oturumu bu öğretmen ders atamasına ait değil.");
                }
            }

            var note = new TeacherNote
            {
                TeacherId = dto.TeacherId,
                TeacherCourseAssignmentId = dto.TeacherCourseAssignmentId,
                LessonSessionId = dto.LessonSessionId,
                Content = dto.Content.Trim(),
                NoteDate = dto.NoteDate ?? DateTime.Now
            };

            await _teacherNoteRepository.AddAsync(note);
            await _teacherNoteRepository.SaveChangesAsync();

            var createdNote = await _teacherNoteRepository.GetByIdWithIncludePathsAsync(note.Id, IncludePaths);

            return ServiceResponse<TeacherNoteListDto>.Ok(
                MapToListDto(createdNote!),
                "Öğretmen notu başarıyla oluşturuldu."
            );
        }

        public async Task<ServiceResponse<TeacherNoteListDto>> UpdateAsync(UpdateTeacherNoteDto dto)
        {
            var note = await _teacherNoteRepository.GetByIdAsync(dto.Id);

            if (note == null)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Güncellenecek öğretmen notu bulunamadı.");
            }

            var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            var assignment = await _assignmentRepository.GetByIdAsync(dto.TeacherCourseAssignmentId);

            if (assignment == null || !assignment.IsActive)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Aktif öğretmen ders ataması bulunamadı.");
            }

            if (assignment.TeacherId != dto.TeacherId)
            {
                return ServiceResponse<TeacherNoteListDto>.Fail("Bu ders ataması seçilen öğretmene ait değil.");
            }

            if (dto.LessonSessionId.HasValue)
            {
                var lessonSession = await _lessonSessionRepository.GetByIdAsync(dto.LessonSessionId.Value);

                if (lessonSession == null)
                {
                    return ServiceResponse<TeacherNoteListDto>.Fail("Ders oturumu bulunamadı.");
                }

                if (lessonSession.TeacherCourseAssignmentId != dto.TeacherCourseAssignmentId)
                {
                    return ServiceResponse<TeacherNoteListDto>.Fail("Seçilen ders oturumu bu öğretmen ders atamasına ait değil.");
                }
            }

            note.TeacherId = dto.TeacherId;
            note.TeacherCourseAssignmentId = dto.TeacherCourseAssignmentId;
            note.LessonSessionId = dto.LessonSessionId;
            note.Content = dto.Content.Trim();
            note.NoteDate = dto.NoteDate;

            _teacherNoteRepository.Update(note);
            await _teacherNoteRepository.SaveChangesAsync();

            var updatedNote = await _teacherNoteRepository.GetByIdWithIncludePathsAsync(note.Id, IncludePaths);

            return ServiceResponse<TeacherNoteListDto>.Ok(
                MapToListDto(updatedNote!),
                "Öğretmen notu başarıyla güncellendi."
            );
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var note = await _teacherNoteRepository.GetByIdAsync(id);

            if (note == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek öğretmen notu bulunamadı.");
            }

            _teacherNoteRepository.SoftDelete(note);
            await _teacherNoteRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Öğretmen notu başarıyla silindi.");
        }

        private static TeacherNoteListDto MapToListDto(TeacherNote note)
        {
            return new TeacherNoteListDto
            {
                Id = note.Id,
                TeacherId = note.TeacherId,
                TeacherFullName = $"{note.Teacher.FirstName} {note.Teacher.LastName}",

                TeacherCourseAssignmentId = note.TeacherCourseAssignmentId,
                CourseId = note.TeacherCourseAssignment.CourseId,
                CourseName = note.TeacherCourseAssignment.Course.Name,
                ClassGroupId = note.TeacherCourseAssignment.ClassGroupId,
                ClassGroupName = note.TeacherCourseAssignment.ClassGroup.Name,

                LessonSessionId = note.LessonSessionId,
                LessonDate = note.LessonSession?.LessonDate,

                Content = note.Content,
                NoteDate = note.NoteDate
            };
        }
    }
}
