using FluentValidation;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos;

public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli bir ders Id değeri gönderilmelidir.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ders adı boş bırakılamaz.")
            .MaximumLength(100).WithMessage("Ders adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");

        RuleFor(x => x.EducationLevel)
            .Must(x => Enum.IsDefined(typeof(EducationLevel), x))
            .WithMessage("Geçerli bir eğitim seviyesi seçiniz.");
    }
}