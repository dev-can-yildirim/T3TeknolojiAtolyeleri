using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherCourseAssignmentDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.TeacherCourseAssignmentValidators
{
    public class UpdateTeacherCourseAssignmentDtoValidator : AbstractValidator<UpdateTeacherCourseAssignmentDto>
    {
        public UpdateTeacherCourseAssignmentDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir atama Id değeri gönderilmelidir.");

            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen seçilmelidir.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Geçerli bir ders seçilmelidir.");

            RuleFor(x => x.ClassGroupId)
                .GreaterThan(0).WithMessage("Geçerli bir sınıf/grup seçilmelidir.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi boş bırakılamaz.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        }
    }
}
