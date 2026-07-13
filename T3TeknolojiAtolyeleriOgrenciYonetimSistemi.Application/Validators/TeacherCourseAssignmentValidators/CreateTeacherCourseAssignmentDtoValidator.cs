using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.TeacherCourseAssignmentValidators
{
    public class CreateTeacherCourseAssignmentDtoValidator : AbstractValidator<CreateTeacherCourseAssignmentDto>
    {
        public CreateTeacherCourseAssignmentDtoValidator()
        {
            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen seçilmelidir.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Geçerli bir ders seçilmelidir.");

            RuleFor(x => x.ClassGroupId)
                .GreaterThan(0).WithMessage("Geçerli bir sınıf/grup seçilmelidir.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        }
    }
}
