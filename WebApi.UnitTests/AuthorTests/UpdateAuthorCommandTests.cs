using Moq;
using WebApi.AuthorOperations.UpdateAuthor;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class UpdateAuthorCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly UpdateAuthorCommand _command;

    public UpdateAuthorCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _command = new UpdateAuthorCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_WhenAuthorExists_ShouldUpdateAuthor()
    {
        // Arrange
        var author = new Author 
        { 
            Id = 1, 
            FirstName = "John", 
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        var authors = new List<Author> { author }.AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        var model = new UpdateAuthorModel 
        { 
            FirstName = "Jane",
            LastName = "Smith",
            BirthDate = new DateTime(1985, 1, 1)
        };
        _command.AuthorId = 1;
        _command.Model = model;

        // Act
        _command.Handle();

        // Assert
        Assert.Equal("Jane", author.FirstName);
        Assert.Equal("Smith", author.LastName);
        Assert.Equal(new DateTime(1985, 1, 1), author.BirthDate);
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
        _command.Model = new UpdateAuthorModel();

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