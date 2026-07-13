using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.TeacherValidators
{
    public class CreateTeacherDtoValidator : AbstractValidator<CreateTeacherDto>
    {
        public CreateTeacherDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Öğretmen adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Öğretmen adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Öğretmen soyadı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Öğretmen soyadı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
                .MaximumLength(150).WithMessage("Email adresi en fazla 150 karakter olabilir.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Şifre en fazla 50 karakter olabilir.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

            RuleFor(x => x.Branch)
                .MaximumLength(100).WithMessage("Branş en fazla 100 karakter olabilir.");
        }
    }
}
