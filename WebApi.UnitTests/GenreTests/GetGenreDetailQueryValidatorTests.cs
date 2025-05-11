using FluentValidation.TestHelper;
using WebApi.GenreOperations.GetGenreDetail;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class GetGenreDetailQueryValidatorTests
{
    private readonly GetGenreDetailQueryValidator _validator;

    public GetGenreDetailQueryValidatorTests()
    {
        _validator = new GetGenreDetailQueryValidator();
    }

    [Fact]
    public void GenreId_WhenZero_ShouldHaveValidationError()
    {
        var model = new GetGenreDetailQuery(null, null) { GenreId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new GetGenreDetailQuery(null, null) { GenreId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new GetGenreDetailQuery(null, null) { GenreId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.GenreId);
    }
} 