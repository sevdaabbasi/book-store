using FluentValidation.TestHelper;
using WebApi.AuthorOperations.CreateAuthor;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class CreateAuthorCommandValidatorTests
{
    private readonly CreateAuthorCommandValidator _validator;

    public CreateAuthorCommandValidatorTests()
    {
        _validator = new CreateAuthorCommandValidator();
    }

    [Fact]
    public void Model_WhenFirstNameIsEmpty_ShouldHaveValidationError()
    {
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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
        var model = new CreateAuthorCommand(null, null)
        {
            Model = new CreateAuthorModel 
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