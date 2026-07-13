using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TeachersController : BaseApiController 
    {
        private readonly ITeacherService _teacherService;
        private readonly IValidator<CreateTeacherDto> _createTeacherValidator;
        private readonly IValidator<UpdateTeacherDto> _updateTeacherValidator;

        public TeachersController(
            ITeacherService teacherService,
            IValidator<CreateTeacherDto> createTeacherValidator,
            IValidator<UpdateTeacherDto> updateTeacherValidator)
        {
            _teacherService = teacherService;
            _createTeacherValidator = createTeacherValidator;
            _updateTeacherValidator = updateTeacherValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _teacherService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _teacherService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherDto dto)
        {
            var validationResult = await _createTeacherValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _teacherService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTeacherDto dto)
        {
            var validationResult = await _updateTeacherValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _teacherService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _teacherService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
