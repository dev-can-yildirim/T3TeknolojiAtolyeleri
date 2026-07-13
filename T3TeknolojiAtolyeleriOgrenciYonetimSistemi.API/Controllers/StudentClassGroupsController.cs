using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StudentClassGroupsController : BaseApiController
    {
        private readonly IStudentClassGroupService _studentClassGroupService;
        private readonly IValidator<CreateStudentClassGroupDto> _createValidator;
        private readonly IValidator<UpdateStudentClassGroupDto> _updateValidator;

        public StudentClassGroupsController(
            IStudentClassGroupService studentClassGroupService,
            IValidator<CreateStudentClassGroupDto> createValidator,
            IValidator<UpdateStudentClassGroupDto> updateValidator)
        {
            _studentClassGroupService = studentClassGroupService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _studentClassGroupService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _studentClassGroupService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("class-group/{classGroupId}")]
        public async Task<IActionResult> GetByClassGroupId(int classGroupId)
        {
            var response = await _studentClassGroupService.GetByClassGroupIdAsync(classGroupId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var response = await _studentClassGroupService.GetByStudentIdAsync(studentId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentClassGroupDto dto)
        {
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

            var response = await _studentClassGroupService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateStudentClassGroupDto dto)
        {
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

            var response = await _studentClassGroupService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _studentClassGroupService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
