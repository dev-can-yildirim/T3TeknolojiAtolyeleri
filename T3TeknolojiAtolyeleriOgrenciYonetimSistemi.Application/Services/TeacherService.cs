using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Security;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _teacherRepository;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public TeacherService(
            IGenericRepository<Teacher> teacherRepository,
            IGenericRepository<AppUser> userRepository,
            IPasswordHasherService passwordHasherService)
        {
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<ServiceResponse<List<TeacherListDto>>> GetAllAsync()
        {
            var teachers = await _teacherRepository.GetAllWithIncludesAsync(x => x.AppUser);

            var result = teachers
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x => new TeacherListDto
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    FullName = $"{x.FirstName} {x.LastName}",
                    Email = x.AppUser.Email,
                    PhoneNumber = x.PhoneNumber,
                    Branch = x.Branch,
                    IsActive = x.IsActive
                })
                .ToList();

            return ServiceResponse<List<TeacherListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherListDto>> GetByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdWithIncludesAsync(id, x => x.AppUser);

            if (teacher == null)
            {
                return ServiceResponse<TeacherListDto>.Fail("Öğretmen bulunamadı.");
            }

            var result = new TeacherListDto
            {
                Id = teacher.Id,
                AppUserId = teacher.AppUserId,
                FullName = $"{teacher.FirstName} {teacher.LastName}",
                Email = teacher.AppUser.Email,
                PhoneNumber = teacher.PhoneNumber,
                Branch = teacher.Branch,
                IsActive = teacher.IsActive
            };

            return ServiceResponse<TeacherListDto>.Ok(result);
        }

        public async Task<ServiceResponse<TeacherListDto>> CreateAsync(CreateTeacherDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var emailExists = await _userRepository.AnyAsync(x => x.Email.ToLower() == email);

            if (emailExists)
            {
                return ServiceResponse<TeacherListDto>.Fail("Bu email adresiyle kayıtlı bir kullanıcı zaten var.");
            }

            var fullName = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}";

            var appUser = new AppUser
            {
                FullName = fullName,
                Email = email,
                PasswordHash = _passwordHasherService.HashPassword(dto.Password),
                Role = UserRole.Teacher,
                IsActive = true
            };

            await _userRepository.AddAsync(appUser);
            await _userRepository.SaveChangesAsync();

            var teacher = new Teacher
            {
                AppUserId = appUser.Id,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                Branch = dto.Branch?.Trim(),
                IsActive = true
            };

            await _teacherRepository.AddAsync(teacher);
            await _teacherRepository.SaveChangesAsync();

            var result = new TeacherListDto
            {
                Id = teacher.Id,
                AppUserId = appUser.Id,
                FullName = fullName,
                Email = appUser.Email,
                PhoneNumber = teacher.PhoneNumber,
                Branch = teacher.Branch,
                IsActive = teacher.IsActive
            };

            return ServiceResponse<TeacherListDto>.Ok(result, "Öğretmen başarıyla oluşturuldu.");
        }

        public async Task<ServiceResponse<TeacherListDto>> UpdateAsync(UpdateTeacherDto dto)
        {
            var teacher = await _teacherRepository.GetByIdWithIncludesAsync(dto.Id, x => x.AppUser);

            if (teacher == null)
            {
                return ServiceResponse<TeacherListDto>.Fail("Güncellenecek öğretmen bulunamadı.");
            }

            var email = dto.Email.Trim().ToLower();

            var emailExists = await _userRepository.AnyAsync(x =>
                x.Email.ToLower() == email &&
                x.Id != teacher.AppUserId);

            if (emailExists)
            {
                return ServiceResponse<TeacherListDto>.Fail("Bu email adresi başka bir kullanıcı tarafından kullanılıyor.");
            }

            teacher.FirstName = dto.FirstName.Trim();
            teacher.LastName = dto.LastName.Trim();
            teacher.PhoneNumber = dto.PhoneNumber?.Trim();
            teacher.Branch = dto.Branch?.Trim();
            teacher.IsActive = dto.IsActive;

            teacher.AppUser.FullName = $"{teacher.FirstName} {teacher.LastName}";
            teacher.AppUser.Email = email;
            teacher.AppUser.IsActive = dto.IsActive;

            _teacherRepository.Update(teacher);
            _userRepository.Update(teacher.AppUser);

            await _teacherRepository.SaveChangesAsync();

            var result = new TeacherListDto
            {
                Id = teacher.Id,
                AppUserId = teacher.AppUserId,
                FullName = teacher.AppUser.FullName,
                Email = teacher.AppUser.Email,
                PhoneNumber = teacher.PhoneNumber,
                Branch = teacher.Branch,
                IsActive = teacher.IsActive
            };

            return ServiceResponse<TeacherListDto>.Ok(result, "Öğretmen başarıyla güncellendi.");
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdWithIncludesAsync(id, x => x.AppUser);

            if (teacher == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek öğretmen bulunamadı.");
            }

            teacher.IsActive = false;
            teacher.AppUser.IsActive = false;

            _teacherRepository.SoftDelete(teacher);
            _userRepository.SoftDelete(teacher.AppUser);

            await _teacherRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Öğretmen başarıyla silindi.");
        }
    }
}
