using T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface ITeacherService
    {
        Task<ServiceResponse<List<TeacherListDto>>> GetAllAsync();

        Task<ServiceResponse<TeacherListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<TeacherListDto>> CreateAsync(CreateTeacherDto dto);

        Task<ServiceResponse<TeacherListDto>> UpdateAsync(UpdateTeacherDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
