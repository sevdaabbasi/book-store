using FluentValidation.TestHelper;
using WebApi.AuthorOperations.GetAuthorDetail;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class GetAuthorDetailQueryValidatorTests
{
    private readonly GetAuthorDetailQueryValidator _validator;

    public GetAuthorDetailQueryValidatorTests()
    {
        _validator = new GetAuthorDetailQueryValidator();
    }

    [Fact]
    public void AuthorId_WhenZero_ShouldHaveValidationError()
    {
        var model = new GetAuthorDetailQuery(null, null) { AuthorId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new GetAuthorDetailQuery(null, null) { AuthorId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new GetAuthorDetailQuery(null, null) { AuthorId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AuthorId);
    }
} 