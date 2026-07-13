using FluentValidation;
using T3TeknolojiAtolyeleriApplication.DTOs.TeacherNoteDtos;

namespace T3TeknolojiAtolyeleriApplication.Validators.TeacherNoteValidators
{
    public class UpdateTeacherNoteDtoValidator : AbstractValidator<UpdateTeacherNoteDto>
    {
        public UpdateTeacherNoteDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir not Id değeri gönderilmelidir.");

            RuleFor(x => x.TeacherId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen seçilmelidir.");

            RuleFor(x => x.TeacherCourseAssignmentId)
                .GreaterThan(0).WithMessage("Geçerli bir öğretmen ders ataması seçilmelidir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Not içeriği boş bırakılamaz.")
                .MaximumLength(1000).WithMessage("Not içeriği en fazla 1000 karakter olabilir.");

            RuleFor(x => x.NoteDate)
                .NotEmpty().WithMessage("Not tarihi boş bırakılamaz.");

            RuleFor(x => x.LessonSessionId)
                .GreaterThan(0)
                .When(x => x.LessonSessionId.HasValue)
                .WithMessage("Geçerli bir ders oturumu seçilmelidir.");
        }
    }
}
