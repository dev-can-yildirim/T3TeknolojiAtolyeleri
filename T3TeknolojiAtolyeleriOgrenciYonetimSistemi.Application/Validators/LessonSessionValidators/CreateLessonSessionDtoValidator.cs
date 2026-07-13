using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.LessonSessionDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.LessonSessionValidators
{
    public class CreateLessonSessionDtoValidator : AbstractValidator<CreateLessonSessionDto>
    {
        public CreateLessonSessionDtoValidator()
        {
            RuleFor(x => x.TeacherCourseAssignmentId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen ders ataması seçilmelidir.");

            RuleFor(x => x.LessonDate)
                .NotEmpty().WithMessage("Ders tarihi boş bırakılamaz.");

            RuleFor(x => x.Topic)
                .MaximumLength(300).WithMessage("Konu en fazla 300 karakter olabilir.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .When(x => x.StartTime.HasValue && x.EndTime.HasValue)
                .WithMessage("Bitiş saati başlangıç saatinden sonra olmalıdır.");
        }
    }
}
