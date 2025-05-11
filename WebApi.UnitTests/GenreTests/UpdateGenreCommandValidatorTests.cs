using FluentValidation.TestHelper;
using WebApi.GenreOperations.UpdateGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class UpdateGenreCommandValidatorTests
{
    private readonly UpdateGenreCommandValidator _validator;

    public UpdateGenreCommandValidatorTests()
    {
        _validator = new UpdateGenreCommandValidator();
    }

    [Fact]
    public void GenreId_WhenZero_ShouldHaveValidationError()
    {
        var model = new UpdateGenreCommand(null) { GenreId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new UpdateGenreCommand(null) { GenreId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void GenreId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new UpdateGenreCommand(null) { GenreId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.GenreId);
    }

    [Fact]
    public void Model_WhenNameIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateGenreCommand(null)
        {
            GenreId = 1,
            Model = new UpdateGenreModel { Name = string.Empty }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public void Model_WhenNameIsLessThanMinLength_ShouldHaveValidationError()
    {
        var model = new UpdateGenreCommand(null)
        {
            GenreId = 1,
            Model = new UpdateGenreModel { Name = "A" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public void Model_WhenNameIsValid_ShouldNotHaveValidationError()
    {
        var model = new UpdateGenreCommand(null)
        {
            GenreId = 1,
            Model = new UpdateGenreModel { Name = "Valid Genre" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
    }
} 