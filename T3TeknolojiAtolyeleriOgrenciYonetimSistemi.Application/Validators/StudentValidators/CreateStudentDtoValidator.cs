using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.StudentValidators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Öğrenci adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Öğrenci adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Öğrenci soyadı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Öğrenci soyadı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(150).WithMessage("Email adresi en fazla 150 karakter olabilir.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .MinimumLength(10).WithMessage("Telefon numarası en az 10 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

            RuleFor(x => x.EducationLevel)
                .Must(x => Enum.IsDefined(typeof(EducationLevel), x))
                .WithMessage("Geçerli bir eğitim seviyesi seçiniz.");

            RuleFor(x => x.GradeLevel)
                .InclusiveBetween(1, 12)
                .WithMessage("Sınıf seviyesi 1 ile 12 arasında olmalıdır.");
        }
    }
}
