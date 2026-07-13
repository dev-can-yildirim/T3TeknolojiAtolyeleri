using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.DTOs.ClassGroupDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClassGroupsController : BaseApiController
    {
        private readonly IClassGroupService _classGroupService;
        private readonly IValidator<CreateClassGroupDto> _createClassGroupValidator;
        private readonly IValidator<UpdateClassGroupDto> _updateClassGroupValidator;

        public ClassGroupsController(
            IClassGroupService classGroupService,
            IValidator<CreateClassGroupDto> createClassGroupValidator,
            IValidator<UpdateClassGroupDto> updateClassGroupValidator)
        {
            _classGroupService = classGroupService;
            _createClassGroupValidator = createClassGroupValidator;
            _updateClassGroupValidator = updateClassGroupValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _classGroupService.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _classGroupService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClassGroupDto dto)
        {
            var validationResult = await _createClassGroupValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _classGroupService.CreateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateClassGroupDto dto)
        {
            var validationResult = await _updateClassGroupValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _classGroupService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _classGroupService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
