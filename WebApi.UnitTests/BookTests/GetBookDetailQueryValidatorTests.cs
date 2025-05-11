using FluentValidation.TestHelper;
using WebApi.BookOperations.GetBookDetail;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class GetBookDetailQueryValidatorTests
{
    private readonly GetBookDetailQueryValidator _validator;

    public GetBookDetailQueryValidatorTests()
    {
        _validator = new GetBookDetailQueryValidator();
    }

    [Fact]
    public void BookId_WhenZero_ShouldHaveValidationError()
    {
        var model = new GetBookDetailQuery(null, null) { BookId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new GetBookDetailQuery(null, null) { BookId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new GetBookDetailQuery(null, null) { BookId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.BookId);
    }
} 