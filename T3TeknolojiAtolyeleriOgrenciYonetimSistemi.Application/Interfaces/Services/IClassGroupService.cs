using T3TeknolojiAtolyeleriApplication.DTOs.ClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IClassGroupService
    {
        Task<ServiceResponse<List<ClassGroupListDto>>> GetAllAsync();

        Task<ServiceResponse<ClassGroupListDto>> GetByIdAsync(int id);

        Task<ServiceResponse<ClassGroupListDto>> CreateAsync(CreateClassGroupDto dto);

        Task<ServiceResponse<ClassGroupListDto>> UpdateAsync(UpdateClassGroupDto dto);

        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
