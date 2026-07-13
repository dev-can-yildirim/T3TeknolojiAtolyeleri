using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.StudentClassGroupDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.StudentClassGroupValidators
{
    public class CreateStudentClassGroupDtoValidator : AbstractValidator<CreateStudentClassGroupDto>
    {
        public CreateStudentClassGroupDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Geçerli bir öğrenci seçilmelidir.");

            RuleFor(x => x.ClassGroupId)
                .GreaterThan(0).WithMessage("Geçerli bir sınıf/grup seçilmelidir.");
        }
    }
}
