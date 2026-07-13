using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class AttendancesController : BaseApiController
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ILessonSessionService _lessonSessionService;
        private readonly IValidator<SaveAttendanceDto> _saveAttendanceValidator;
        private readonly ICurrentUserService _currentUserService;

        public AttendancesController(
            IAttendanceService attendanceService,
            ILessonSessionService lessonSessionService,
            IValidator<SaveAttendanceDto> saveAttendanceValidator,
            ICurrentUserService currentUserService)
        {
            _attendanceService = attendanceService;
            _lessonSessionService = lessonSessionService;
            _saveAttendanceValidator = saveAttendanceValidator;
            _currentUserService = currentUserService;
        }

        [HttpGet("lesson-session/{lessonSessionId}")]
        public async Task<IActionResult> GetByLessonSessionId(int lessonSessionId)
        {
            var lessonSessionResponse = await _lessonSessionService.GetByIdAsync(lessonSessionId);

            if (!lessonSessionResponse.Success || lessonSessionResponse.Data == null)
            {
                return NotFound(lessonSessionResponse);
            }

            if (_currentUserService.IsTeacher &&
                lessonSessionResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _attendanceService.GetByLessonSessionIdAsync(lessonSessionId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("warnings")]
        public async Task<IActionResult> GetWarningStudents()
        {
            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                var teacherResponse = await _attendanceService.GetWarningStudentsByTeacherIdAsync(
                    _currentUserService.TeacherId.Value
                );

                return Ok(teacherResponse);
            }

            var response = await _attendanceService.GetWarningStudentsAsync();

            return Ok(response);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAttendance(SaveAttendanceDto dto)
        {
            var validationResult = await _saveAttendanceValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var lessonSessionResponse = await _lessonSessionService.GetByIdAsync(dto.LessonSessionId);

            if (!lessonSessionResponse.Success || lessonSessionResponse.Data == null)
            {
                return NotFound(lessonSessionResponse);
            }

            if (_currentUserService.IsTeacher &&
                lessonSessionResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _attendanceService.SaveAttendanceAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}