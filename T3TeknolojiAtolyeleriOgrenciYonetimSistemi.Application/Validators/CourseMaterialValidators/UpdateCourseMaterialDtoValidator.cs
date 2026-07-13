using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseMaterialDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.CourseMaterialValidators
{
    public class UpdateCourseMaterialDtoValidator : AbstractValidator<UpdateCourseMaterialDto>
    {
        public UpdateCourseMaterialDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir materyal Id değeri gönderilmelidir.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Materyal başlığı boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Materyal başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
