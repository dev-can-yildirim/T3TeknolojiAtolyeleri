using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.AuthValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş bırakılamaz.");
        }
    }
}
