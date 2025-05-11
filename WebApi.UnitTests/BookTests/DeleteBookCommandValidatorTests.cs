using FluentValidation.TestHelper;
using WebApi.BookOperations.DeleteBook;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class DeleteBookCommandValidatorTests
{
    private readonly DeleteBookCommandValidator _validator;

    public DeleteBookCommandValidatorTests()
    {
        _validator = new DeleteBookCommandValidator();
    }

    [Fact]
    public void BookId_WhenZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteBookCommand(null) { BookId = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenNegative_ShouldHaveValidationError()
    {
        // Arrange
        var command = new DeleteBookCommand(null) { BookId = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookId);
    }

    [Fact]
    public void BookId_WhenPositive_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new DeleteBookCommand(null) { BookId = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.BookId);
    }
} 