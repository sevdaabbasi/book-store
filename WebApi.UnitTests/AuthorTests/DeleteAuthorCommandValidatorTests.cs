using FluentValidation.TestHelper;
using WebApi.AuthorOperations.DeleteAuthor;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class DeleteAuthorCommandValidatorTests
{
    private readonly DeleteAuthorCommandValidator _validator;

    public DeleteAuthorCommandValidatorTests()
    {
        _validator = new DeleteAuthorCommandValidator();
    }

    [Fact]
    public void AuthorId_WhenZero_ShouldHaveValidationError()
    {
        var model = new DeleteAuthorCommand(null) { AuthorId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new DeleteAuthorCommand(null) { AuthorId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new DeleteAuthorCommand(null) { AuthorId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AuthorId);
    }
} 