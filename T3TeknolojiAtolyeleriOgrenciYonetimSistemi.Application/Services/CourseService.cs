using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Responses;
public class CourseService : ICourseService
{
    private readonly IGenericRepository<Course> _courseRepository;

    public CourseService(IGenericRepository<Course> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<ServiceResponse<List<CourseListDto>>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();

        var result = courses
            .OrderBy(x => x.EducationLevel)
            .ThenBy(x => x.Name)
            .Select(x => new CourseListDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                EducationLevel = x.EducationLevel,
                IsActive = x.IsActive
            })
            .ToList();

        return ServiceResponse<List<CourseListDto>>.Ok(result);
    }

    public async Task<ServiceResponse<CourseListDto>> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course == null)
        {
            return ServiceResponse<CourseListDto>.Fail("Ders bulunamadı.");
        }

        var result = new CourseListDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            EducationLevel = course.EducationLevel,
            IsActive = course.IsActive
        };

        return ServiceResponse<CourseListDto>.Ok(result);
    }

    public async Task<ServiceResponse<CourseListDto>> CreateAsync(CreateCourseDto dto)
    {
        var name = dto.Name.Trim();

        var courseExists = await _courseRepository.AnyAsync(x =>
            x.Name.ToLower() == name.ToLower() &&
            x.EducationLevel == dto.EducationLevel);

        if (courseExists)
        {
            return ServiceResponse<CourseListDto>.Fail("Bu eğitim seviyesinde aynı isimde bir ders zaten mevcut.");
        }

        var course = new Course
        {
            Name = name,
            Description = dto.Description?.Trim(),
            EducationLevel = dto.EducationLevel,
            IsActive = true
        };

        await _courseRepository.AddAsync(course);
        await _courseRepository.SaveChangesAsync();

        var result = new CourseListDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            EducationLevel = course.EducationLevel,
            IsActive = course.IsActive
        };

        return ServiceResponse<CourseListDto>.Ok(result, "Ders başarıyla eklendi.");
    }

    public async Task<ServiceResponse<CourseListDto>> UpdateAsync(UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(dto.Id);

        if (course == null)
        {
            return ServiceResponse<CourseListDto>.Fail("Güncellenecek ders bulunamadı.");
        }

        var name = dto.Name.Trim();

        var courseExists = await _courseRepository.AnyAsync(x =>
            x.Name.ToLower() == name.ToLower() &&
            x.EducationLevel == dto.EducationLevel &&
            x.Id != dto.Id);

        if (courseExists)
        {
            return ServiceResponse<CourseListDto>.Fail("Bu eğitim seviyesinde aynı isimde başka bir ders zaten mevcut.");
        }

        course.Name = name;
        course.Description = dto.Description?.Trim();
        course.EducationLevel = dto.EducationLevel;
        course.IsActive = dto.IsActive;

        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();

        var result = new CourseListDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description,
            EducationLevel = course.EducationLevel,
            IsActive = course.IsActive
        };

        return ServiceResponse<CourseListDto>.Ok(result, "Ders başarıyla güncellendi.");
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);

        if (course == null)
        {
            return ServiceResponse<bool>.Fail("Silinecek ders bulunamadı.");
        }

        _courseRepository.SoftDelete(course);
        await _courseRepository.SaveChangesAsync();

        return ServiceResponse<bool>.Ok(true, "Ders başarıyla silindi.");
    }
}
