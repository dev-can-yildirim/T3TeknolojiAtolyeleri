using T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IStudentClassGroupService
    {
        Task<ServiceResponse<List<StudentClassGroupListDto>>> GetAllAsync();

        Task<ServiceResponse<StudentClassGroupListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<List<StudentClassGroupListDto>>> GetByClassGroupIdAsync(int classGroupId);

        Task<ServiceResponse<List<StudentClassGroupListDto>>> GetByStudentIdAsync(int studentId);

        Task<ServiceResponse<StudentClassGroupListDto>> CreateAsync(CreateStudentClassGroupDto dto);

        Task<ServiceResponse<StudentClassGroupListDto>> UpdateAsync(UpdateStudentClassGroupDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
