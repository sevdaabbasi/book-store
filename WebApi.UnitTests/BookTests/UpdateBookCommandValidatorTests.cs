using FluentValidation.TestHelper;
using WebApi.BookOperations.UpdateBook;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class UpdateBookCommandValidatorTests
{
    private readonly UpdateBookCommandValidator _validator;

    public UpdateBookCommandValidatorTests()
    {
        _validator = new UpdateBookCommandValidator();
    }

    [Fact]
    public void BookId_WhenZero_ShouldHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null) { BookId = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenNegative_ShouldHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null) { BookId = -1 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenPositive_ShouldNotHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null) { BookId = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void Model_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null)
        {
            BookId = 1,
            Model = new UpdateBookModel { Title = string.Empty }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.Title);
    }

    [Fact]
    public void Model_WhenTitleIsValid_ShouldNotHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null)
        {
            BookId = 1,
            Model = new UpdateBookModel { Title = "Valid Title" }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.Title);
    }

    [Fact]
    public void Model_WhenPageCountIsZero_ShouldHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null)
        {
            BookId = 1,
            Model = new UpdateBookModel { PageCount = 0 }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Model.PageCount);
    }

    [Fact]
    public void Model_WhenPageCountIsPositive_ShouldNotHaveValidationError()
    {
        var model = new UpdateBookCommand(null, null)
        {
            BookId = 1,
            Model = new UpdateBookModel { PageCount = 100 }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Model.PageCount);
    }
} 