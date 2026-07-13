using T3TeknolojiAtolyeleriApplication.DTOs.StudentDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IStudentService
    {
        Task<ServiceResponse<List<StudentListDto>>> GetAllAsync();

        Task<ServiceResponse<StudentListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<StudentListDto>> CreateAsync(CreateStudentDto dto);

        Task<ServiceResponse<StudentListDto>> UpdateAsync(UpdateStudentDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
