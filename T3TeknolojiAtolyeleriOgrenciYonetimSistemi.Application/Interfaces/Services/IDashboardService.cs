using T3TeknolojiAtolyeleriApplication.DTOs.DashboardDtos;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<ServiceResponse<DashboardSummaryDto>> GetAdminSummaryAsync();

        Task<ServiceResponse<DashboardSummaryDto>> GetTeacherSummaryAsync(int teacherId);
    }
}
