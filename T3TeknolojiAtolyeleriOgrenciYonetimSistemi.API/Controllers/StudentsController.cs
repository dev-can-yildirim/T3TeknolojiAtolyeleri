using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StudentsController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<CreateStudentDto> _createStudentValidator;
        private readonly IValidator<UpdateStudentDto> _updateStudentValidator;

        public StudentsController(
            IStudentService studentService,
            IValidator<CreateStudentDto> createStudentValidator,
            IValidator<UpdateStudentDto> updateStudentValidator)
        {
            _studentService = studentService;
            _createStudentValidator = createStudentValidator;
            _updateStudentValidator = updateStudentValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _studentService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _studentService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentDto dto)
        {
            var validationResult = await _createStudentValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _studentService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateStudentDto dto)
        {
            var validationResult = await _updateStudentValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _studentService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}