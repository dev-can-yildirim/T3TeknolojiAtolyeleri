using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class LessonSessionsController : BaseApiController
    {
        private readonly ILessonSessionService _lessonSessionService;
        private readonly ITeacherCourseAssignmentService _assignmentService;
        private readonly IValidator<CreateLessonSessionDto> _createValidator;
        private readonly IValidator<UpdateLessonSessionDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;

        public LessonSessionsController(
            ILessonSessionService lessonSessionService,
            ITeacherCourseAssignmentService assignmentService,
            IValidator<CreateLessonSessionDto> createValidator,
            IValidator<UpdateLessonSessionDto> updateValidator,
            ICurrentUserService currentUserService)
        {
            _lessonSessionService = lessonSessionService;
            _assignmentService = assignmentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _lessonSessionService.GetAllAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                response.Data = response.Data?
                    .Where(x => x.TeacherId == _currentUserService.TeacherId.Value)
                    .ToList();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _lessonSessionService.GetByIdAsync(id);

            if (!response.Success || response.Data == null)
            {
                return NotFound(response);
            }

            if (_currentUserService.IsTeacher &&
                response.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            return Ok(response);
        }

        [HttpGet("assignment/{teacherCourseAssignmentId}")]
        public async Task<IActionResult> GetByAssignmentId(int teacherCourseAssignmentId)
        {
            var assignmentResponse = await _assignmentService.GetByIdAsync(teacherCourseAssignmentId);

            if (!assignmentResponse.Success || assignmentResponse.Data == null)
            {
                return NotFound(assignmentResponse);
            }

            if (_currentUserService.IsTeacher &&
                assignmentResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _lessonSessionService.GetByAssignmentIdAsync(teacherCourseAssignmentId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLessonSessionDto dto)
        {
            var assignmentResponse = await _assignmentService.GetByIdAsync(dto.TeacherCourseAssignmentId);

            if (!assignmentResponse.Success || assignmentResponse.Data == null)
            {
                return NotFound(assignmentResponse);
            }

            if (_currentUserService.IsTeacher &&
                assignmentResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _lessonSessionService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateLessonSessionDto dto)
        {
            var existingSessionResponse = await _lessonSessionService.GetByIdAsync(dto.Id);

            if (!existingSessionResponse.Success || existingSessionResponse.Data == null)
            {
                return NotFound(existingSessionResponse);
            }

            if (_currentUserService.IsTeacher &&
                existingSessionResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var assignmentResponse = await _assignmentService.GetByIdAsync(dto.TeacherCourseAssignmentId);

            if (!assignmentResponse.Success || assignmentResponse.Data == null)
            {
                return NotFound(assignmentResponse);
            }

            if (_currentUserService.IsTeacher &&
                assignmentResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _lessonSessionService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingSessionResponse = await _lessonSessionService.GetByIdAsync(id);

            if (!existingSessionResponse.Success || existingSessionResponse.Data == null)
            {
                return NotFound(existingSessionResponse);
            }

            if (_currentUserService.IsTeacher &&
                existingSessionResponse.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _lessonSessionService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}