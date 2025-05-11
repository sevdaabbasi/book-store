using AutoMapper;
using Moq;
using WebApi.AuthorOperations.GetAuthorDetail;
using WebApi.DbOperations;
using Xunit;

namespace WebApi.UnitTests.AuthorTests;

public class GetAuthorDetailQueryTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetAuthorDetailQuery _query;

    public GetAuthorDetailQueryTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _query = new GetAuthorDetailQuery(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenAuthorExists_ShouldReturnAuthorDetail()
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

        var expectedDto = new AuthorDetailViewModel 
        { 
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1)
        };
        _mockMapper.Setup(x => x.Map<AuthorDetailViewModel>(author)).Returns(expectedDto);

        _query.AuthorId = 1;

        // Act
        var result = _query.Handle();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.FirstName, result.FirstName);
        Assert.Equal(expectedDto.LastName, result.LastName);
        Assert.Equal(expectedDto.BirthDate, result.BirthDate);
    }

    [Fact]
    public void Handle_WhenAuthorDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var authors = new List<Author>().AsQueryable();
        var mockDbSet = MockDbSet(authors);
        _mockContext.Setup(x => x.Authors).Returns(mockDbSet.Object);

        _query.AuthorId = 1;

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