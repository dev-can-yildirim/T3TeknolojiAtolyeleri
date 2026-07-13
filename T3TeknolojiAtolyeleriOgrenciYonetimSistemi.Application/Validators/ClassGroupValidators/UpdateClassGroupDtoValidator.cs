using FluentValidation;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.ClassGroupDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.ClassGroupValidators
{
    public class UpdateClassGroupDtoValidator : AbstractValidator<UpdateClassGroupDto>
    {
        public UpdateClassGroupDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir sınıf/grup Id değeri gönderilmelidir.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Sınıf/grup adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Sınıf/grup adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.EducationLevel)
                .Must(x => Enum.IsDefined(typeof(EducationLevel), x))
                .WithMessage("Geçerli bir eğitim seviyesi seçiniz.");

            RuleFor(x => x.GradeLevel)
                .InclusiveBetween(1, 12)
                .WithMessage("Sınıf seviyesi 1 ile 12 arasında olmalıdır.");

            RuleFor(x => x.AcademicYear)
                .MaximumLength(20).WithMessage("Akademik yıl en fazla 20 karakter olabilir.");
        }
    }
}
