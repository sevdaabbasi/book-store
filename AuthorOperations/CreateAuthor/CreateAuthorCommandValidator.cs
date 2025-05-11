using FluentValidation;
using WebApi;

namespace WebApi.AuthorOperations.CreateAuthor;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorDto>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.BirthDate).NotEmpty().LessThan(DateTime.Now);
    }
} 