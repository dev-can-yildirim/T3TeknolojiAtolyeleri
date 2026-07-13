using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;

        public StudentService(IGenericRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ServiceResponse<List<StudentListDto>>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();

            var result = students
                .OrderBy(x => x.FirstName)
                .Select(x => new StudentListDto
                {
                    Id = x.Id,
                    FullName = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    EducationLevel = x.EducationLevel,
                    GradeLevel = x.GradeLevel,
                    HasPurchasedEducationSet = x.HasPurchasedEducationSet,
                    IsActive = x.IsActive
                })
                .ToList();

            return ServiceResponse<List<StudentListDto>>.Ok(result);
        }

        public async Task<ServiceResponse<StudentListDto>> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return ServiceResponse<StudentListDto>.Fail("Öğrenci bulunamadı.");
            }

            var result = new StudentListDto
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                EducationLevel = student.EducationLevel,
                GradeLevel = student.GradeLevel,
                HasPurchasedEducationSet = student.HasPurchasedEducationSet,
                IsActive = student.IsActive
            };

            return ServiceResponse<StudentListDto>.Ok(result);
        }

        public async Task<ServiceResponse<StudentListDto>> CreateAsync(CreateStudentDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var emailExists = await _studentRepository.AnyAsync(x => x.Email.ToLower() == email);

            if (emailExists)
            {
                return ServiceResponse<StudentListDto>.Fail("Bu email adresi zaten kullanılıyor.");
            }

            var student = new Student
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = email,
                PhoneNumber = dto.PhoneNumber.Trim(),
                EducationLevel = dto.EducationLevel,
                GradeLevel = dto.GradeLevel,
                HasPurchasedEducationSet = dto.HasPurchasedEducationSet,
                IsActive = true
            };

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            var result = new StudentListDto
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                EducationLevel = student.EducationLevel,
                GradeLevel = student.GradeLevel,
                HasPurchasedEducationSet = student.HasPurchasedEducationSet,
                IsActive = student.IsActive
            };

            return ServiceResponse<StudentListDto>.Ok(result, "Öğrenci başarıyla eklendi.");
        }
        public async Task<ServiceResponse<StudentListDto>> UpdateAsync(UpdateStudentDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(dto.Id);

            if (student == null)
            {
                return ServiceResponse<StudentListDto>.Fail("Güncellenecek öğrenci bulunamadı.");
            }

            var email = dto.Email.Trim().ToLower();

            var emailExists = await _studentRepository.AnyAsync(x =>
                x.Email.ToLower() == email && x.Id != dto.Id);

            if (emailExists)
            {
                return ServiceResponse<StudentListDto>.Fail("Bu email adresi başka bir öğrenci tarafından kullanılıyor.");
            }

            student.FirstName = dto.FirstName.Trim();
            student.LastName = dto.LastName.Trim();
            student.Email = email;
            student.PhoneNumber = dto.PhoneNumber.Trim();
            student.EducationLevel = dto.EducationLevel;
            student.GradeLevel = dto.GradeLevel;
            student.HasPurchasedEducationSet = dto.HasPurchasedEducationSet;
            student.IsActive = dto.IsActive;

            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();

            var result = new StudentListDto
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                EducationLevel = student.EducationLevel,
                GradeLevel = student.GradeLevel,
                HasPurchasedEducationSet = student.HasPurchasedEducationSet,
                IsActive = student.IsActive
            };

            return ServiceResponse<StudentListDto>.Ok(result, "Öğrenci başarıyla güncellendi.");
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return ServiceResponse<bool>.Fail("Silinecek öğrenci bulunamadı.");
            }

            _studentRepository.SoftDelete(student);
            await _studentRepository.SaveChangesAsync();

            return ServiceResponse<bool>.Ok(true, "Öğrenci başarıyla silindi.");
        }
    }
}
