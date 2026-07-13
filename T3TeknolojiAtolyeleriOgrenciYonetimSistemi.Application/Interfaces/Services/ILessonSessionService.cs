using T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface ILessonSessionService
    {
        Task<ServiceResponse<List<LessonSessionListDto>>> GetAllAsync();

        Task<ServiceResponse<LessonSessionListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<List<LessonSessionListDto>>> GetByAssignmentIdAsync(int teacherCourseAssignmentId);

        Task<ServiceResponse<LessonSessionListDto>> CreateAsync(CreateLessonSessionDto dto);

        Task<ServiceResponse<LessonSessionListDto>> UpdateAsync(UpdateLessonSessionDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
