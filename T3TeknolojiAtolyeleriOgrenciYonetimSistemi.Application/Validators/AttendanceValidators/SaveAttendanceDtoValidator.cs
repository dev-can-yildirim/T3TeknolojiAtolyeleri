using FluentValidation;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.AttendanceDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.AttendanceValidators
{
    public class SaveAttendanceDtoValidator : AbstractValidator<SaveAttendanceDto>
    {
        public SaveAttendanceDtoValidator()
        {
            RuleFor(x => x.LessonSessionId)
                .GreaterThan(0).WithMessage("Geçerli bir ders oturumu seçilmelidir.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Yoklama listesi boş olamaz.");

            RuleFor(x => x.Items)
                .Must(items => items.Select(x => x.StudentId).Distinct().Count() == items.Count)
                .WithMessage("Aynı öğrenci yoklama listesinde birden fazla kez bulunamaz.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.StudentId)
                    .GreaterThan(0).WithMessage("Geçerli bir öğrenci seçilmelidir.");

                item.RuleFor(x => x.Status)
                    .Must(x => Enum.IsDefined(typeof(AttendanceStatus), x))
                    .WithMessage("Geçerli bir yoklama durumu seçilmelidir.");

                item.RuleFor(x => x.Description)
                    .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
            });
        }
    }
}
