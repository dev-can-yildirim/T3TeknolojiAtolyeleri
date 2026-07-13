using T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface ICourseMaterialService
    {
        Task<ServiceResponse<List<CourseMaterialListDto>>> GetAllAsync();

        Task<ServiceResponse<CourseMaterialListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<List<CourseMaterialListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId);

        Task<ServiceResponse<List<CourseMaterialListDto>>> GetByTeacherIdAsync(int teacherId);

        Task<ServiceResponse<CourseMaterialListDto>> CreateAsync(CreateCourseMaterialDto dto);

        Task<ServiceResponse<CourseMaterialListDto>> UpdateAsync(UpdateCourseMaterialDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
