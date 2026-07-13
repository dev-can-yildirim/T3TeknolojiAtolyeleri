using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly IGenericRepository<CourseMaterial> _courseMaterialRepository;
        private readonly IGenericRepository<TeacherCourseAssignment> _assignmentRepository;
        private readonly IGenericRepository<Teacher> _teacherRepository;

        private static readonly string[] IncludePaths =
        {
            "TeacherCourseAssignment.Teacher",
            "TeacherCourseAssignment.Course",
            "TeacherCourseAssignment.ClassGroup",
            "UploadedByTeacher"
        };

        public CourseMaterialService(
            IGenericRepository<CourseMaterial> courseMaterialRepository,
            IGenericRepository<TeacherCourseAssignment> assignmentRepository,
            IGenericRepository<Teacher> teacherRepository)
        {
            _courseMaterialRepository = courseMaterialRepository;
            _assignmentRepository = assignmentRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task<ServiceResponse<List<CourseMaterialListDto>>> GetAllAsync()
        {
            var materials = await _courseMaterialRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = materials
                .OrderByDescending(x => x.UploadedAt)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<CourseMaterialListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<CourseMaterialListDto>> GetByIdAsync(int id)
        {
            var material = await _courseMaterialRepository.GetByIdWithIncludePathsAsync(id, IncludePaths);

            if (material == null)
            {
                return ServiceResponse<CourseMaterialListDto>.Fail("Materyal bulunamadı.");
            }

            return ServiceResponse<CourseMaterialListDto>.Ok(MapToListDto(material));
        }

        public async Task<ServiceResponse<List<CourseMaterialListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(teacherCourseAssignmentId);

            if (assignment == null)
            {
                return ServiceResponse<List<CourseMaterialListDto>>.Fail("Öğretmen ders ataması bulunamadı.");
            }

            var materials = await _courseMaterialRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = materials
                .Where(x => x.TeacherCourseAssignmentId == teacherCourseAssignmentId && x.IsActive)
                .OrderByDescending(x => x.UploadedAt)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<CourseMaterialListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<List<CourseMaterialListDto>>> GetByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                return ServiceResponse<List<CourseMaterialListDto>>.Fail("Öğretmen bulunamadı.");
            }

            var materials = await _courseMaterialRepository.GetAllWithIncludePathsAsync(IncludePaths);

            var result = materials
                .Where(x => x.UploadedByTeacherId == teacherId && x.IsActive)
                .OrderByDescending(x => x.UploadedAt)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<CourseMaterialListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<CourseMaterialListDto>> CreateAsync(CreateCourseMaterialDto dto)
        {
            var assignment = await _assignmentRepository.GetByIdWithIncludesAsync(
                dto.TeacherCourseAssignmentId,
                x => x.Teacher,
                x => x.Course,
                x => x.ClassGroup
            );

            if (assignment == null || !assignment.IsActive)
            {
                return ServiceResponse<CourseMaterialListDto>.Fail("Aktif öğretmen ders ataması bulunamadı.");
            }

            var teacher = await _teacherRepository.GetByIdAsync(dto.UploadedByTeacherId);

            if (teacher == null || !teacher.IsActive)
            {
                return ServiceResponse<CourseMaterialListDto>.Fail("Aktif öğretmen bulunamadı.");
            }

            if (assignment.TeacherId != dto.UploadedByTeacherId)
            {
                return ServiceResponse<CourseMaterialListDto>.Fail("Bu öğretmen seçilen ders atamasına ait materyal yükleyemez.");
            }

            var material = new CourseMaterial
            {
                TeacherCourseAssignmentId = dto.TeacherCourseAssignmentId,
                UploadedByTeacherId = dto.UploadedByTeacherId,
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                FilePath = dto.FilePath,
                OriginalFileName = dto.OriginalFileName,
                ContentType = dto.ContentType,
                FileSize = dto.FileSize,
                MaterialType = dto.MaterialType,
                UploadedAt = DateTime.Now,
                IsActive = true
            };

            await _courseMaterialRepository.AddAsync(material);
            await _courseMaterialRepository.SaveChangesAsync();

            var createdMaterial = await _courseMaterialRepository.GetByIdWithIncludePathsAsync(material.Id, IncludePaths);

            return ServiceResponse<CourseMaterialListDto>.Ok(
                MapToListDto(createdMaterial!),
                "Materyal başarıyla yüklendi."
            );
        }

        public async Task<ServiceResponse<CourseMaterialListDto>> UpdateAsync(UpdateCourseMaterialDto dto)
        {
            var material = await _courseMaterialRepository.GetByIdWithIncludePathsAsync(dto.Id, IncludePaths);

            if (material == null)
            {
                return ServiceResponse<CourseMaterialListDto>.Fail("Güncellenecek materyal bulunamadı.");
            }

            material.Title = dto.Title.Trim();
            material.Description = dto.Description?.Trim();
            material.IsActive = dto.IsActive;

            _courseMaterialRepository.Update(material);
            await _courseMaterialRepository.SaveChangesAsync();

            var updatedMaterial = await _courseMaterialRepository.GetByIdWithIncludePathsAsync(material.Id, IncludePaths);

            return ServiceResponse<CourseMaterialListDto>.Ok(
                MapToListDto(updatedMaterial!),
                "Materyal başarıyla güncellendi."
            );
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var material = await _courseMaterialRepository.GetByIdAsync(id);

            if (material == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek materyal bulunamadı.");
            }

            material.IsActive = false;

            _courseMaterialRepository.SoftDelete(material);
            await _courseMaterialRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Materyal başarıyla silindi.");
        }

        private static CourseMaterialListDto MapToListDto(CourseMaterial material)
        {
            var assignment = material.TeacherCourseAssignment;

            return new CourseMaterialListDto
            {
                Id = material.Id,
                TeacherCourseAssignmentId = material.TeacherCourseAssignmentId,

                TeacherId = assignment.TeacherId,
                TeacherFullName = $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",

                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,

                ClassGroupId = assignment.ClassGroupId,
                ClassGroupName = assignment.ClassGroup.Name,

                UploadedByTeacherId = material.UploadedByTeacherId,
                UploadedByTeacherFullName = $"{material.UploadedByTeacher.FirstName} {material.UploadedByTeacher.LastName}",

                Title = material.Title,
                Description = material.Description,
                FilePath = material.FilePath,
                OriginalFileName = material.OriginalFileName,
                ContentType = material.ContentType,
                FileSize = material.FileSize,
                MaterialType = material.MaterialType,
                UploadedAt = material.UploadedAt,
                IsActive = material.IsActive
            };
        }
    }
}
