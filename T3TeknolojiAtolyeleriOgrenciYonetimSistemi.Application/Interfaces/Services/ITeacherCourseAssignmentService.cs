using T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface ITeacherCourseAssignmentService
    {
        Task<ServiceResponse<List<TeacherCourseAssignmentListDto>>> GetAllAsync();

        Task<ServiceResponse<TeacherCourseAssignmentListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<List<TeacherCourseAssignmentListDto>>> GetByTeacherIdAsync(int teacherId);

        Task<ServiceResponse<TeacherCourseAssignmentListDto>> CreateAsync(CreateTeacherCourseAssignmentDto dto);

        Task<ServiceResponse<TeacherCourseAssignmentListDto>> UpdateAsync(UpdateTeacherCourseAssignmentDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
