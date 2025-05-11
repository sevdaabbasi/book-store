using FluentValidation.TestHelper;
using WebApi.AuthorOperations.UpdateAuthor;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class UpdateAuthorCommandValidatorTests
{
    private readonly UpdateAuthorCommandValidator _validator;

    public UpdateAuthorCommandValidatorTests()
    {
        _validator = new UpdateAuthorCommandValidator();
    }

    [Fact]
    public void AuthorId_WhenZero_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null) { AuthorId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null) { AuthorId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void AuthorId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null) { AuthorId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AuthorId);
    }

    [Fact]
    public void Model_WhenFirstNameIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = string.Empty,
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.FirstName);
    }

    [Fact]
    public void Model_WhenFirstNameIsLessThanMinLength_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = "J",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.FirstName);
    }

    [Fact]
    public void Model_WhenLastNameIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = "John",
                LastName = string.Empty,
                BirthDate = new DateTime(1980, 1, 1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.LastName);
    }

    [Fact]
    public void Model_WhenLastNameIsLessThanMinLength_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = "John",
                LastName = "D",
                BirthDate = new DateTime(1980, 1, 1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.LastName);
    }

    [Fact]
    public void Model_WhenBirthDateIsInFuture_ShouldHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddDays(1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.BirthDate);
    }

    [Fact]
    public void Model_WhenAllFieldsAreValid_ShouldNotHaveValidationError()
    {
        var model = new UpdateAuthorCommand(null)
        {
            AuthorId = 1,
            Model = new UpdateAuthorModel 
            { 
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.FirstName);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.LastName);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.BirthDate);
    }
} 