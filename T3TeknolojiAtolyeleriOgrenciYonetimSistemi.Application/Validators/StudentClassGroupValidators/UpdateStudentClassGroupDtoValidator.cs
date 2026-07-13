using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.StudentClassGroupValidators
{
    public class UpdateStudentClassGroupDtoValidator : AbstractValidator<UpdateStudentClassGroupDto>
    {
        public UpdateStudentClassGroupDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir kayıt Id değeri gönderilmelidir.");

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Geçerli bir öğrenci seçilmelidir.");

            RuleFor(x => x.ClassGroupId)
                .GreaterThan(0).WithMessage("Geçerli bir sınıf/grup seçilmelidir.");
        }
    }
}
