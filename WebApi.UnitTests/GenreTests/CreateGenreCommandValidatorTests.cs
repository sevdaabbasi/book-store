using FluentValidation.TestHelper;
using WebApi.GenreOperations.CreateGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class CreateGenreCommandValidatorTests
{
    private readonly CreateGenreCommandValidator _validator;

    public CreateGenreCommandValidatorTests()
    {
        _validator = new CreateGenreCommandValidator();
    }

    [Fact]
    public void Model_WhenNameIsEmpty_ShouldHaveValidationError()
    {
        var model = new CreateGenreCommand(null, null)
        {
            Model = new CreateGenreModel { Name = string.Empty }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public void Model_WhenNameIsLessThanMinLength_ShouldHaveValidationError()
    {
        var model = new CreateGenreCommand(null, null)
        {
            Model = new CreateGenreModel { Name = "A" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.Name);
    }

    [Fact]
    public void Model_WhenNameIsValid_ShouldNotHaveValidationError()
    {
        var model = new CreateGenreCommand(null, null)
        {
            Model = new CreateGenreModel { Name = "Valid Genre" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Name);
    }
} 