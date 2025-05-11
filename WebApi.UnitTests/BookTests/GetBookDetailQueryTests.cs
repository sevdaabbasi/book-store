using AutoMapper;
using Moq;
using WebApi.BookOperations.GetBookDetail;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.BookTests;

public class GetBookDetailQueryTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetBookDetailQuery _query;

    public GetBookDetailQueryTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _query = new GetBookDetailQuery(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenBookExists_ShouldReturnBookDetail()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book" };
        var books = new List<Book> { book }.AsQueryable();
        var mockDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);

        var expectedDto = new BookDetailViewModel { Title = "Test Book" };
        _mockMapper.Setup(x => x.Map<BookDetailViewModel>(book)).Returns(expectedDto);

        _query.BookId = 1;

        // Act
        var result = _query.Handle();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Title, result.Title);
    }

    [Fact]
    public void Handle_WhenBookDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var books = new List<Book>().AsQueryable();
        var mockDbSet = MockDbSet(books);
        _mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);

        _query.BookId = 1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _query.Handle());
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