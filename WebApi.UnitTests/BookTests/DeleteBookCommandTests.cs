using AutoMapper;
using Moq;
using WebApi.BookOperations.DeleteBook;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class DeleteBookCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DeleteBookCommand _command;

    public DeleteBookCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _command = new DeleteBookCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_WhenBookExists_ShouldDeleteBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book" };
        var books = new List<Book> { book }.AsQueryable();
        var mockDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);
        _command.BookId = 1;

        // Act
        _command.Handle();

        // Assert
        _mockContext.Verify(x => x.Books.Remove(It.IsAny<Book>()), Times.Once);
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Handle_WhenBookDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var books = new List<Book>().AsQueryable();
        var mockDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);
        _command.BookId = 1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _command.Handle());
    }

    private Mock<DbSet<T>> MockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }
} 