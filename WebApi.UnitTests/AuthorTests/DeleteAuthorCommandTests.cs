using Moq;
using WebApi.AuthorOperations.DeleteAuthor;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class DeleteAuthorCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly DeleteAuthorCommand _command;

    public DeleteAuthorCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _command = new DeleteAuthorCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_WhenAuthorExistsAndHasNoBooks_ShouldDeleteAuthor()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var authors = new List<Author> { author }.AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        var books = new List<Book>().AsQueryable();
        var mockBookDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockBookDbSet.Object);

        _command.AuthorId = 1;

        // Act
        _command.Handle();

        // Assert
        _mockContext.Verify(x => x.Authors.Remove(It.IsAny<Author>()), Times.Once);
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Handle_WhenAuthorDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var authors = new List<Author>().AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        _command.AuthorId = 1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _command.Handle());
    }

    [Fact]
    public void Handle_WhenAuthorHasBooks_ShouldThrowException()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var authors = new List<Author> { author }.AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        var book = new Book { Id = 1, AuthorId = 1 };
        var books = new List<Book> { book }.AsQueryable();
        var mockBookDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockBookDbSet.Object);

        _command.AuthorId = 1;

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