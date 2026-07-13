using FluentValidation;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.CourseMaterialValidators
{
    public class CreateCourseMaterialDtoValidator : AbstractValidator<CreateCourseMaterialDto>
    {
        public CreateCourseMaterialDtoValidator()
        {
            RuleFor(x => x.TeacherCourseAssignmentId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen ders ataması seçilmelidir.");

            RuleFor(x => x.UploadedByTeacherId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen seçilmelidir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Materyal başlığı boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Materyal başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("Dosya yolu boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Dosya yolu en fazla 500 karakter olabilir.");

            RuleFor(x => x.OriginalFileName)
                .NotEmpty().WithMessage("Orijinal dosya adı boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Orijinal dosya adı en fazla 200 karakter olabilir.");

            RuleFor(x => x.ContentType)
                .MaximumLength(100).WithMessage("Dosya içerik tipi en fazla 100 karakter olabilir.");

            RuleFor(x => x.FileSize)
                .GreaterThan(0).WithMessage("Dosya boyutu geçerli olmalıdır.");

            RuleFor(x => x.MaterialType)
                .Must(x => Enum.IsDefined(typeof(MaterialType), x))
                .WithMessage("Geçerli bir materyal tipi seçilmelidir.");
        }
    }
}
