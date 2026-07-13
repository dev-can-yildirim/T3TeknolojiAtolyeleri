using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.TeacherValidators
{
    public class UpdateTeacherDtoValidator : AbstractValidator<UpdateTeacherDto>
    {
        public UpdateTeacherDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen Id değeri gönderilmelidir.");

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

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.");

            RuleFor(x => x.Branch)
                .MaximumLength(100).WithMessage("Branş en fazla 100 karakter olabilir.");
        }
    }
}
