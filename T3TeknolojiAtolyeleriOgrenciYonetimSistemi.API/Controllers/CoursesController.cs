using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CoursesController : BaseApiController
    {
        private readonly ICourseService _courseService;
        private readonly IValidator<CreateCourseDto> _createCourseValidator;
        private readonly IValidator<UpdateCourseDto> _updateCourseValidator;

        public CoursesController(
            ICourseService courseService,
            IValidator<CreateCourseDto> createCourseValidator,
            IValidator<UpdateCourseDto> updateCourseValidator)
        {
            _courseService = courseService;
            _createCourseValidator = createCourseValidator;
            _updateCourseValidator = updateCourseValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _courseService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var validationResult = await _createCourseValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _courseService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseDto dto)
        {
            var validationResult = await _updateCourseValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _courseService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _courseService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
