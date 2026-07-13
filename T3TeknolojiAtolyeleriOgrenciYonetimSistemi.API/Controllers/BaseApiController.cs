using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ValidationErrorResult(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .Select(x => x.ErrorMessage)
                .ToList();

            var response = ServiceResponse<object>.Fail(
                "Validasyon hatası oluştu.",
                errors
            );

            return BadRequest(response);
        }

        protected IActionResult SuccessResult<T>(T data, string? message = null)
        {
            return Ok(ServiceResponse<T>.Ok(data, message));
        }

        protected IActionResult BadRequestResult<T>(string message)
        {
            return BadRequest(ServiceResponse<T>.Fail(message));
        }

        protected IActionResult NotFoundResult<T>(string message)
        {
            return NotFound(ServiceResponse<T>.Fail(message));
        }
    }
}