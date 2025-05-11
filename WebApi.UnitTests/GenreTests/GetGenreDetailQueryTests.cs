using AutoMapper;
using Moq;
using WebApi.DbOperations;
using WebApi.GenreOperations.GetGenreDetail;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class GetGenreDetailQueryTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetGenreDetailQuery _query;

    public GetGenreDetailQueryTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _query = new GetGenreDetailQuery(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenGenreExists_ShouldReturnGenreDetail()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Test Genre" };
        var genres = new List<Genre> { genre }.AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        var expectedDto = new GenreDetailViewModel { Name = "Test Genre" };
        _mockMapper.Setup(x => x.Map<GenreDetailViewModel>(genre)).Returns(expectedDto);

        _query.GenreId = 1;

        // Act
        var result = _query.Handle();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Name, result.Name);
    }

    [Fact]
    public void Handle_WhenGenreDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var genres = new List<Genre>().AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        _query.GenreId = 1;

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