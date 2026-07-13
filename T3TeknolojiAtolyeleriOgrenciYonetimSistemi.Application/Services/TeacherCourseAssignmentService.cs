using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class TeacherCourseAssignmentService : ITeacherCourseAssignmentService
    {
        private readonly IGenericRepository<TeacherCourseAssignment> _assignmentRepository;
        private readonly IGenericRepository<Teacher> _teacherRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<ClassGroup> _classGroupRepository;

        public TeacherCourseAssignmentService(
            IGenericRepository<TeacherCourseAssignment> assignmentRepository,
            IGenericRepository<Teacher> teacherRepository,
            IGenericRepository<Course> courseRepository,
            IGenericRepository<ClassGroup> classGroupRepository)
        {
            _assignmentRepository = assignmentRepository;
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
            _classGroupRepository = classGroupRepository;
        }

        public async Task<ServiceResponse<List<TeacherCourseAssignmentListDto>>> GetAllAsync()
        {
            var assignments = await _assignmentRepository.GetAllWithIncludesAsync(
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            var result = assignments
                .OrderBy(x => x.Teacher.FirstName)
                .ThenBy(x => x.Course.Name)
                .ThenBy(x => x.ClassGroup.Name)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<TeacherCourseAssignmentListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherCourseAssignmentListDto>> GetByIdAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                id,
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            if (assignment == null)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Atama bulunamadı.");
            }

            return ServiceResponse<TeacherCourseAssignmentListDto>.Ok(MapToListDto(assignment));
        }

        public async Task<ServiceResponse<List<TeacherCourseAssignmentListDto>>> GetByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                return ServiceResponse<List<TeacherCourseAssignmentListDto>>.Fail("Öğretmen bulunamadı.");
            }

            var assignments = await _assignmentRepository.GetAllWithIncludesAsync(
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            var result = assignments
                .Where(x => x.TeacherId == teacherId && x.IsActive)
                .OrderBy(x => x.Course.Name)
                .ThenBy(x => x.ClassGroup.Name)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<TeacherCourseAssignmentListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherCourseAssignmentListDto>> CreateAsync(CreateTeacherCourseAssignmentDto dto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            var course = await _courseRepository.GetByIdAsync(dto.CourseId);

            if (course == null || !course.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif ders bulunamadı.");
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(dto.ClassGroupId);

            if (classGroup == null || !classGroup.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif sınıf/grup bulunamadı.");
            }

            if (course.EducationLevel != classGroup.EducationLevel)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Dersin eğitim seviyesi ile sınıf/grubun eğitim seviyesi uyumlu değil.");
            }

            var assignmentExists = await _assignmentRepository.AnyAsync(x =>
                x.TeacherId == dto.TeacherId &&
                x.CourseId == dto.CourseId &&
                x.ClassGroupId == dto.ClassGroupId &&
                x.IsActive);

            if (assignmentExists)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Bu öğretmen için aynı ders ve sınıf/grup ataması zaten mevcut.");
            }

            var assignment = new TeacherCourseAssignment
            {
                TeacherId = dto.TeacherId,
                CourseId = dto.CourseId,
                ClassGroupId = dto.ClassGroupId,
                StartDate = dto.StartDate ?? DateTime.Now,
                EndDate = dto.EndDate,
                IsActive = true
            };

            await _assignmentRepository.AddAsync(assignment);
            await _assignmentRepository.SaveChangesAsync();

            var createdAssignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                assignment.Id,
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            return ServiceResponse<TeacherCourseAssignmentListDto>.Ok(
                MapToListDto(createdAssignment!),
                "Öğretmen ders ataması başarıyla oluşturuldu."
            );
        }

        public async Task<ServiceResponse<TeacherCourseAssignmentListDto>> UpdateAsync(UpdateTeacherCourseAssignmentDto dto)
        {
            var assignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                dto.Id,
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            if (assignment == null)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Güncellenecek atama bulunamadı.");
            }

            var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            var course = await _courseRepository.GetByIdAsync(dto.CourseId);

            if (course == null || !course.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif ders bulunamadı.");
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(dto.ClassGroupId);

            if (classGroup == null || !classGroup.IsActive)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Aktif sınıf/grup bulunamadı.");
            }

            if (course.EducationLevel != classGroup.EducationLevel)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Dersin eğitim seviyesi ile sınıf/grubun eğitim seviyesi uyumlu değil.");
            }

            var assignmentExists = await _assignmentRepository.AnyAsync(x =>
                x.TeacherId == dto.TeacherId &&
                x.CourseId == dto.CourseId &&
                x.ClassGroupId == dto.ClassGroupId &&
                x.Id != dto.Id &&
                x.IsActive);

            if (assignmentExists)
            {
                return ServiceResponse<TeacherCourseAssignmentListDto>.Fail("Bu bilgilerle başka bir aktif atama zaten mevcut.");
            }

            assignment.TeacherId = dto.TeacherId;
            assignment.CourseId = dto.CourseId;
            assignment.ClassGroupId = dto.ClassGroupId;
            assignment.StartDate = dto.StartDate;
            assignment.EndDate = dto.EndDate;
            assignment.IsActive = dto.IsActive;

            _assignmentRepository.Update(assignment);
            await _assignmentRepository.SaveChangesAsync();

            var updatedAssignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                assignment.Id,
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            return ServiceResponse<TeacherCourseAssignmentListDto>.Ok(
                MapToListDto(updatedAssignment!),
                "Öğretmen ders ataması başarıyla güncellendi."
            );
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);

            if (assignment == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek atama bulunamadı.");
            }

            assignment.IsActive = false;

            _assignmentRepository.SoftDelete(assignment);
            await _assignmentRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Öğretmen ders ataması başarıyla silindi.");
        }

        private static TeacherCourseAssignmentListDto MapToListDto(TeacherCourseAssignment assignment)
        {
            return new TeacherCourseAssignmentListDto
            {
                Id = assignment.Id,
                TeacherId = assignment.TeacherId,
                TeacherFullName = $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",
                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,
                ClassGroupId = assignment.ClassGroupId,
                ClassGroupName = assignment.ClassGroup.Name,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate,
                IsActive = assignment.IsActive
            };
        }
    }
}
