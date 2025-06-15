using FluentValidation;
using Notes.Application.Models.Requests;

namespace Notes.API.Validators
{
    public class PatchNoteRequestValidator : AbstractValidator<PatchNoteRequest>
    {
        public PatchNoteRequestValidator()
        {
            RuleFor(x => x.Title)
                .MinimumLength(3);
        }
    }
}
