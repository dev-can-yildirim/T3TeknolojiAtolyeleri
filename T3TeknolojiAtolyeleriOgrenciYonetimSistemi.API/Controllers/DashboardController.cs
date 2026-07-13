using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICurrentUserService _currentUserService;

        public DashboardController(
            IDashboardService dashboardService,
            ICurrentUserService currentUserService)
        {
            _dashboardService = dashboardService;
            _currentUserService = currentUserService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            if (_currentUserService.IsAdmin)
            {
                var adminResponse = await _dashboardService.GetAdminSummaryAsync();
                return Ok(adminResponse);
            }

            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                var teacherResponse = await _dashboardService.GetTeacherSummaryAsync(
                    _currentUserService.TeacherId.Value
                );

                if (!teacherResponse.Success)
                {
                    return BadRequest(teacherResponse);
                }

                return Ok(teacherResponse);
            }

            return Forbid();
        }
    }
}