using T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

public interface ICourseService
{
    Task<ServiceResponse<List<CourseListDto>>> GetAllAsync();

    Task<ServiceResponse<CourseListDto>> GetByIdAsync(int id);

    Task<ServiceResponse<CourseListDto>> CreateAsync(CreateCourseDto dto);

    Task<ServiceResponse<CourseListDto>> UpdateAsync(UpdateCourseDto dto);

    Task<ServiceResponse<bool>> DeleteAsync(int id);
}