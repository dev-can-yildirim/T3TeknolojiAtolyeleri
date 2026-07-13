using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using T3TeknolojiAtolyeleri.API.Requests;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;

namespace T3TeknolojiAtolyeleri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Teacher")]
    public class CourseMaterialsController : BaseApiController
    {
        private readonly ICourseMaterialService _courseMaterialService;
        private readonly IValidator<CreateCourseMaterialDto> _createValidator;
        private readonly IValidator<UpdateCourseMaterialDto> _updateValidator;
        private readonly IWebHostEnvironment _environment;
        private readonly ICurrentUserService _currentUserService;

        private const long MaxFileSize = 20 * 1024 * 1024; // 20 MB

        public CourseMaterialsController(
            ICourseMaterialService courseMaterialService,
            IValidator<CreateCourseMaterialDto> createValidator,
            IValidator<UpdateCourseMaterialDto> updateValidator,
            IWebHostEnvironment environment,
            ICurrentUserService currentUserService)
        {
            _courseMaterialService = courseMaterialService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _environment = environment;
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

                var teacherResponse = await _courseMaterialService.GetByTeacherIdAsync(_currentUserService.TeacherId.Value);
                return Ok(teacherResponse);
            }

            var response = await _courseMaterialService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _courseMaterialService.GetByIdAsync(id);

            if (!response.Success || response.Data == null)
            {
                return NotFound(response);
            }

            if (_currentUserService.IsTeacher &&
                response.Data.UploadedByTeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            return Ok(response);
        }

        [HttpGet("assignment/{teacherCourseAssignmentId}")]
        public async Task<IActionResult> GetByAssignmentId(int teacherCourseAssignmentId)
        {
            var response = await _courseMaterialService.GetByAssignmentIdAsync(teacherCourseAssignmentId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            if (_currentUserService.IsTeacher)
            {
                response.Data = response.Data?
                    .Where(x => x.UploadedByTeacherId == _currentUserService.TeacherId)
                    .ToList();
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

            var response = await _courseMaterialService.GetByTeacherIdAsync(teacherId);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadCourseMaterialRequest request)
        {
            if (_currentUserService.IsTeacher)
            {
                if (!_currentUserService.TeacherId.HasValue)
                {
                    return Forbid();
                }

                request.UploadedByTeacherId = _currentUserService.TeacherId.Value;
            }

            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Dosya seçilmelidir."
                });
            }

            if (request.File.Length > MaxFileSize)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Dosya boyutu en fazla 20 MB olabilir."
                });
            }

            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

            var materialType = GetMaterialType(extension);

            if (materialType == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Sadece PDF, Word ve PowerPoint dosyaları yüklenebilir."
                });
            }

            var uploadsFolder = Path.Combine(
                _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                "uploads",
                "materials"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var safeOriginalFileName = Path.GetFileName(request.File.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var physicalPath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var relativeFilePath = $"uploads/materials/{uniqueFileName}";

            var dto = new CreateCourseMaterialDto
            {
                TeacherCourseAssignmentId = request.TeacherCourseAssignmentId,
                UploadedByTeacherId = request.UploadedByTeacherId,
                Title = request.Title,
                Description = request.Description,
                FilePath = relativeFilePath,
                OriginalFileName = safeOriginalFileName,
                ContentType = request.File.ContentType,
                FileSize = request.File.Length,
                MaterialType = materialType.Value
            };

            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Validasyon hatası oluştu.",
                    errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            var response = await _courseMaterialService.CreateAsync(dto);

            if (!response.Success)
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseMaterialDto dto)
        {
            var existingMaterial = await _courseMaterialService.GetByIdAsync(dto.Id);

            if (!existingMaterial.Success || existingMaterial.Data == null)
            {
                return NotFound(existingMaterial);
            }

            if (_currentUserService.IsTeacher &&
                existingMaterial.Data.UploadedByTeacherId != _currentUserService.TeacherId)
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

            var response = await _courseMaterialService.UpdateAsync(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var response = await _courseMaterialService.GetByIdAsync(id);

            if (!response.Success || response.Data == null)
            {
                return NotFound(response);
            }

            var material = response.Data;

            if (_currentUserService.IsTeacher &&
                material.UploadedByTeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var physicalPath = Path.Combine(
                webRootPath,
                material.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (!System.IO.File.Exists(physicalPath))
            {
                return NotFound(new
                {
                    success = false,
                    message = "Dosya sunucuda bulunamadı."
                });
            }

            return PhysicalFile(
                physicalPath,
                material.ContentType ?? "application/octet-stream",
                material.OriginalFileName
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingMaterial = await _courseMaterialService.GetByIdAsync(id);

            if (!existingMaterial.Success || existingMaterial.Data == null)
            {
                return NotFound(existingMaterial);
            }

            if (_currentUserService.IsTeacher &&
                existingMaterial.Data.UploadedByTeacherId != _currentUserService.TeacherId)
            {
                return Forbid();
            }

            var response = await _courseMaterialService.DeleteAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        private static MaterialType? GetMaterialType(string extension)
        {
            return extension switch
            {
                ".pdf" => MaterialType.Pdf,
                ".doc" => MaterialType.Word,
                ".docx" => MaterialType.Word,
                ".ppt" => MaterialType.Presentation,
                ".pptx" => MaterialType.Presentation,
                _ => null
            };
        }
    }
}