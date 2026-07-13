using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class StudentClassGroupService : IStudentClassGroupService
    {
        private readonly IGenericRepository<StudentClassGroup> _studentClassGroupRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<ClassGroup> _classGroupRepository;

        public StudentClassGroupService(
            IGenericRepository<StudentClassGroup> studentClassGroupRepository,
            IGenericRepository<Student> studentRepository,
            IGenericRepository<ClassGroup> classGroupRepository)
        {
            _studentClassGroupRepository = studentClassGroupRepository;
            _studentRepository = studentRepository;
            _classGroupRepository = classGroupRepository;
        }

        public async Task<ServiceResponse<List<StudentClassGroupListDto>>> GetAllAsync()
        {
            var records = await _studentClassGroupRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.ClassGroup
            );

            var result = records
                .OrderBy(x => x.ClassGroup.Name)
                .ThenBy(x => x.Student.FirstName)
                .ThenBy(x => x.Student.LastName)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<StudentClassGroupListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<StudentClassGroupListDto>> GetByIdAsync(int id)
        {
            var record = await _studentClassGroupRepository.GetByIdWithIncludesAsync(
                id,
                x => x.Student,
                x => x.ClassGroup
            );

            if (record == null)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Öğrenci sınıf/grup kaydı bulunamadı.");
            }

            return ServiceResponse<StudentClassGroupListDto>.Ok(MapToListDto(record));
        }

        public async Task<ServiceResponse<List<StudentClassGroupListDto>>> GetByClassGroupIdAsync(int classGroupId)
        {
            var classGroup = await _classGroupRepository.GetByIdAsync(classGroupId);

            if (classGroup == null)
            {
                return ServiceResponse<List<StudentClassGroupListDto>>.Fail("Sınıf/grup bulunamadı.");
            }

            var records = await _studentClassGroupRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.ClassGroup
            );

            var result = records
                .Where(x => x.ClassGroupId == classGroupId && x.IsActive)
                .OrderBy(x => x.Student.FirstName)
                .ThenBy(x => x.Student.LastName)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<StudentClassGroupListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<List<StudentClassGroupListDto>>> GetByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                return ServiceResponse<List<StudentClassGroupListDto>>.Fail("Öğrenci bulunamadı.");
            }

            var records = await _studentClassGroupRepository.GetAllWithIncludesAsync(
                x => x.Student,
                x => x.ClassGroup
            );

            var result = records
                .Where(x => x.StudentId == studentId && x.IsActive)
                .OrderBy(x => x.ClassGroup.Name)
                .Select(MapToListDto)
                .ToList();

            return ServiceResponse<List<StudentClassGroupListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<StudentClassGroupListDto>> CreateAsync(CreateStudentClassGroupDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(dto.StudentId);

            if (student == null || !student.IsActive)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Aktif öğrenci bulunamadı.");
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(dto.ClassGroupId);

            if (classGroup == null || !classGroup.IsActive)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Aktif sınıf/grup bulunamadı.");
            }

            if (student.EducationLevel != classGroup.EducationLevel)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Öğrencinin eğitim seviyesi ile sınıf/grubun eğitim seviyesi uyumlu değil.");
            }

            if (student.GradeLevel != classGroup.GradeLevel)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Öğrencinin sınıf seviyesi ile sınıf/grup seviyesi uyumlu değil.");
            }

            var recordExists = await _studentClassGroupRepository.AnyAsync(x =>
                x.StudentId == dto.StudentId &&
                x.ClassGroupId == dto.ClassGroupId &&
                x.IsActive);

            if (recordExists)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Bu öğrenci zaten bu sınıf/gruba kayıtlı.");
            }

            var record = new StudentClassGroup
            {
                StudentId = dto.StudentId,
                ClassGroupId = dto.ClassGroupId,
                EnrollmentDate = DateTime.Now,
                IsActive = true
            };

            await _studentClassGroupRepository.AddAsync(record);
            await _studentClassGroupRepository.SaveChangesAsync();

            var createdRecord = await _studentClassGroupRepository.GetByIdWithIncludesAsync(
                record.Id,
                x => x.Student,
                x => x.ClassGroup
            );

            return ServiceResponse<StudentClassGroupListDto>.Ok(
                MapToListDto(createdRecord!),
                "Öğrenci sınıf/gruba başarıyla eklendi."
            );
        }

        public async Task<ServiceResponse<StudentClassGroupListDto>> UpdateAsync(UpdateStudentClassGroupDto dto)
        {
            var record = await _studentClassGroupRepository.GetByIdWithIncludesAsync(
                dto.Id,
                x => x.Student,
                x => x.ClassGroup
            );

            if (record == null)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Güncellenecek öğrenci sınıf/grup kaydı bulunamadı.");
            }

            var student = await _studentRepository.GetByIdAsync(dto.StudentId);

            if (student == null || !student.IsActive)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Aktif öğrenci bulunamadı.");
            }

            var classGroup = await _classGroupRepository.GetByIdAsync(dto.ClassGroupId);

            if (classGroup == null || !classGroup.IsActive)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Aktif sınıf/grup bulunamadı.");
            }

            if (student.EducationLevel != classGroup.EducationLevel)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Öğrencinin eğitim seviyesi ile sınıf/grubun eğitim seviyesi uyumlu değil.");
            }

            if (student.GradeLevel != classGroup.GradeLevel)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Öğrencinin sınıf seviyesi ile sınıf/grup seviyesi uyumlu değil.");
            }

            var recordExists = await _studentClassGroupRepository.AnyAsync(x =>
                x.StudentId == dto.StudentId &&
                x.ClassGroupId == dto.ClassGroupId &&
                x.Id != dto.Id &&
                x.IsActive);

            if (recordExists)
            {
                return ServiceResponse<StudentClassGroupListDto>.Fail("Bu öğrenci bu sınıf/gruba zaten kayıtlı.");
            }

            record.StudentId = dto.StudentId;
            record.ClassGroupId = dto.ClassGroupId;
            record.IsActive = dto.IsActive;

            _studentClassGroupRepository.Update(record);
            await _studentClassGroupRepository.SaveChangesAsync();

            var updatedRecord = await _studentClassGroupRepository.GetByIdWithIncludesAsync(
                record.Id,
                x => x.Student,
                x => x.ClassGroup
            );

            return ServiceResponse<StudentClassGroupListDto>.Ok(
                MapToListDto(updatedRecord!),
                "Öğrenci sınıf/grup kaydı başarıyla güncellendi."
            );
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var record = await _studentClassGroupRepository.GetByIdAsync(id);

            if (record == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek öğrenci sınıf/grup kaydı bulunamadı.");
            }

            record.IsActive = false;

            _studentClassGroupRepository.SoftDelete(record);
            await _studentClassGroupRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Öğrenci sınıf/gruptan başarıyla çıkarıldı.");
        }

        private static StudentClassGroupListDto MapToListDto(StudentClassGroup record)
        {
            return new StudentClassGroupListDto
            {
                Id = record.Id,
                StudentId = record.StudentId,
                StudentFullName = $"{record.Student.FirstName} {record.Student.LastName}",
                StudentEmail = record.Student.Email,
                StudentPhoneNumber = record.Student.PhoneNumber,
                ClassGroupId = record.ClassGroupId,
                ClassGroupName = record.ClassGroup.Name,
                EnrollmentDate = record.EnrollmentDate,
                IsActive = record.IsActive
            };
        }
    }
}
