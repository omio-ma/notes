using FluentValidation;
using Notes.API.Requests;

namespace Notes.API.Validators
{
    public class NoteRequestValidator : AbstractValidator<NoteRequest>
    {
        public NoteRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(x => x.Content)
                .NotEmpty();
        }
    }
}
