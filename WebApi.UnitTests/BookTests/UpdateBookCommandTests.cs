using AutoMapper;
using Moq;
using WebApi.BookOperations.UpdateBook;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class UpdateBookCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateBookCommand _command;

    public UpdateBookCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _command = new UpdateBookCommand(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenBookExists_ShouldUpdateBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Old Title" };
        var books = new List<Book> { book }.AsQueryable();
        var mockDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);
        
        var updateModel = new UpdateBookModel { Title = "New Title" };
        _command.BookId = 1;
        _command.Model = updateModel;

        // Act
        _command.Handle();

        // Assert
        _mockMapper.Verify(x => x.Map(updateModel, book), Times.Once);
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
        _command.Model = new UpdateBookModel();

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