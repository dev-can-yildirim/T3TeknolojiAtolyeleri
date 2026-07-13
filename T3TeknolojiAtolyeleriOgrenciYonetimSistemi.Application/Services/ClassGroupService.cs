using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.ClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class ClassGroupService : IClassGroupService
    {
        private readonly IGenericRepository<ClassGroup> _classGroupRepository;

        public ClassGroupService(IGenericRepository<ClassGroup> classGroupRepository)
        {
            _classGroupRepository = classGroupRepository;
        }

        public async Task<ServiceResponse<List<ClassGroupListDto>>> GetAllAsync()
        {
            var classGroups = await _classGroupRepository.GetAllAsync();

            var result = classGroups
                .OrderBy(x => x.EducationLevel)
                .ThenBy(x => x.GradeLevel)
                .ThenBy(x => x.Name)
                .Select(x => new ClassGroupListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    EducationLevel = x.EducationLevel,
                    GradeLevel = x.GradeLevel,
                    AcademicYear = x.AcademicYear,
                    IsActive = x.IsActive
                })
                .ToList();

            return ServiceResponse<List<ClassGroupListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<ClassGroupListDto>> GetByIdAsync(int id)
        {
            var classGroup = await _classGroupRepository.GetByIdAsync(id);

            if (classGroup == null)
            {
                return ServiceResponse<ClassGroupListDto>.Fail("Sınıf/grup bulunamadı.");
            }

            var result = new ClassGroupListDto
            {
                Id = classGroup.Id,
                Name = classGroup.Name,
                EducationLevel = classGroup.EducationLevel,
                GradeLevel = classGroup.GradeLevel,
                AcademicYear = classGroup.AcademicYear,
                IsActive = classGroup.IsActive
            };

            return ServiceResponse<ClassGroupListDto>.Ok(result);
        }

        public async Task<ServiceResponse<ClassGroupListDto>> CreateAsync(CreateClassGroupDto dto)
        {
            var name = dto.Name.Trim();
            var academicYear = dto.AcademicYear?.Trim();

            var classGroupExists = await _classGroupRepository.AnyAsync(x =>
                x.Name.ToLower() == name.ToLower() &&
                x.EducationLevel == dto.EducationLevel &&
                x.GradeLevel == dto.GradeLevel &&
                x.AcademicYear == academicYear);

            if (classGroupExists)
            {
                return ServiceResponse<ClassGroupListDto>.Fail("Bu sınıf/grup zaten mevcut.");
            }

            var classGroup = new ClassGroup
            {
                Name = name,
                EducationLevel = dto.EducationLevel,
                GradeLevel = dto.GradeLevel,
                AcademicYear = academicYear,
                IsActive = true
            };

            await _classGroupRepository.AddAsync(classGroup);
            await _classGroupRepository.SaveChangesAsync();

            var result = new ClassGroupListDto
            {
                Id = classGroup.Id,
                Name = classGroup.Name,
                EducationLevel = classGroup.EducationLevel,
                GradeLevel = classGroup.GradeLevel,
                AcademicYear = classGroup.AcademicYear,
                IsActive = classGroup.IsActive
            };

            return ServiceResponse<ClassGroupListDto>.Ok(result, "Sınıf/grup başarıyla eklendi.");
        }

        public async Task<ServiceResponse<ClassGroupListDto>> UpdateAsync(UpdateClassGroupDto dto)
        {
            var classGroup = await _classGroupRepository.GetByIdAsync(dto.Id);

            if (classGroup == null)
            {
                return ServiceResponse<ClassGroupListDto>.Fail("Güncellenecek sınıf/grup bulunamadı.");
            }

            var name = dto.Name.Trim();
            var academicYear = dto.AcademicYear?.Trim();

            var classGroupExists = await _classGroupRepository.AnyAsync(x =>
                x.Name.ToLower() == name.ToLower() &&
                x.EducationLevel == dto.EducationLevel &&
                x.GradeLevel == dto.GradeLevel &&
                x.AcademicYear == academicYear &&
                x.Id != dto.Id);

            if (classGroupExists)
            {
                return ServiceResponse<ClassGroupListDto>.Fail("Bu bilgilerle başka bir sınıf/grup zaten mevcut.");
            }

            classGroup.Name = name;
            classGroup.EducationLevel = dto.EducationLevel;
            classGroup.GradeLevel = dto.GradeLevel;
            classGroup.AcademicYear = academicYear;
            classGroup.IsActive = dto.IsActive;

            _classGroupRepository.Update(classGroup);
            await _classGroupRepository.SaveChangesAsync();

            var result = new ClassGroupListDto
            {
                Id = classGroup.Id,
                Name = classGroup.Name,
                EducationLevel = classGroup.EducationLevel,
                GradeLevel = classGroup.GradeLevel,
                AcademicYear = classGroup.AcademicYear,
                IsActive = classGroup.IsActive
            };

            return ServiceResponse<ClassGroupListDto>.Ok(result, "Sınıf/grup başarıyla güncellendi.");
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var classGroup = await _classGroupRepository.GetByIdAsync(id);

            if (classGroup == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek sınıf/grup bulunamadı.");
            }

            _classGroupRepository.SoftDelete(classGroup);
            await _classGroupRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Sınıf/grup başarıyla silindi.");
        }
    }
}
