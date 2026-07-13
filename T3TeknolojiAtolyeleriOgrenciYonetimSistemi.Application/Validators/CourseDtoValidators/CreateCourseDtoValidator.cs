using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.CourseDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.CreateCourseDtoValidator
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
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
}
