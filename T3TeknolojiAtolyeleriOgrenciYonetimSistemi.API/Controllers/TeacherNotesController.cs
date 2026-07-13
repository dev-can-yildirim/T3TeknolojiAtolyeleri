using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class TeacherNotesController : BaseApiController
    {
        private readonly ITeacherNoteService _teacherNoteService;
        private readonly IValidator<CreateTeacherNoteDto> _createValidator;
        private readonly IValidator<UpdateTeacherNoteDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;

        public TeacherNotesController(
            ITeacherNoteService teacherNoteService,
            IValidator<CreateTeacherNoteDto> createValidator,
            IValidator<UpdateTeacherNoteDto> updateValidator,
            ICurrentUserService currentUserService)
        {
            _teacherNoteService = teacherNoteService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                var teacherResponse = await _teacherNoteService.GetByTeacherIdAsync(_currentUserService.TeacherId.Value);
                return Ok(teacherResponse);
            }

            var response = await _teacherNoteService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _teacherNoteService.GetByIdAsync(id);

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

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetByTeacherId(int teacherId)
        {
            if (_currentUserService.IsTeacher &&
                teacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _teacherNoteService.GetByTeacherIdAsync(teacherId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("assignment/{teacherCourseAssignmentId}")]
        public async Task<IActionResult> GetByAssignmentId(int teacherCourseAssignmentId)
        {
            var response = await _teacherNoteService.GetByAssignmentIdAsync(teacherCourseAssignmentId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            if (_currentUserService.IsTeacher)
            {
                response.Data = response.Data?
                    .Where(x => x.TeacherId == _currentUserService.TeacherId)
                    .ToList();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherNoteDto dto)
        {
            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                dto.TeacherId = _currentUserService.TeacherId.Value;
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

            var response = await _teacherNoteService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTeacherNoteDto dto)
        {
            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                if (dto.TeacherId != _currentUserService.TeacherId.Value)
                {
                    return Forbid();
                }
            }

            var existingNote = await _teacherNoteService.GetByIdAsync(dto.Id);

            if (!existingNote.Success || existingNote.Data == null)
            {
                return NotFound(existingNote);
            }

            if (_currentUserService.IsTeacher &&
                existingNote.Data.TeacherId != _currentUserService.TeacherId)
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

            var response = await _teacherNoteService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingNote = await _teacherNoteService.GetByIdAsync(id);

            if (!existingNote.Success || existingNote.Data == null)
            {
                return NotFound(existingNote);
            }

            if (_currentUserService.IsTeacher &&
                existingNote.Data.TeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _teacherNoteService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}