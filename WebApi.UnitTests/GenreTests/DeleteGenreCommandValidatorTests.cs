using FluentValidation.TestHelper;
using WebApi.GenreOperations.DeleteGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class DeleteGenreCommandValidatorTests
{
    private readonly DeleteGenreCommandValidator _validator;

    public DeleteGenreCommandValidatorTests()
    {
        _validator = new DeleteGenreCommandValidator();
    }

    [Fact]
    public void GenreId_WhenZero_ShouldHaveValidationError()
    {
        var model = new DeleteGenreCommand(null) { GenreId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new DeleteGenreCommand(null) { GenreId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new DeleteGenreCommand(null) { GenreId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.GenreId);
    }
} 