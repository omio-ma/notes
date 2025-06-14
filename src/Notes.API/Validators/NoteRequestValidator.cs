using FluentValidation;
using Notes.Application.Models.Requests;

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
