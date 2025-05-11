using AutoMapper;
using Moq;
using WebApi.AuthorOperations.CreateAuthor;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class CreateAuthorCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateAuthorCommand _command;

    public CreateAuthorCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _command = new CreateAuthorCommand(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenAuthorNameIsUnique_ShouldCreateAuthor()
    {
        // Arrange
        var authors = new List<Author>().AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        var model = new CreateAuthorModel 
        { 
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        var author = new Author 
        { 
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockMapper.Setup(x => x.Map<Author>(model)).Returns(author);

        _command.Model = model;

        // Act
        _command.Handle();

        // Assert
        _mockContext.Verify(x => x.Authors.Add(It.IsAny<Author>()), Times.Once);
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Handle_WhenAuthorNameExists_ShouldThrowException()
    {
        // Arrange
        var existingAuthor = new Author 
        { 
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        var authors = new List<Author> { existingAuthor }.AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        var model = new CreateAuthorModel 
        { 
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        _command.Model = model;

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