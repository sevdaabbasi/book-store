using AutoMapper;
using Moq;
using WebApi.DbOperations;
using WebApi.GenreOperations.CreateGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class CreateGenreCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateGenreCommand _command;

    public CreateGenreCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _mockMapper = new Mock<IMapper>();
        _command = new CreateGenreCommand(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public void Handle_WhenGenreNameIsUnique_ShouldCreateGenre()
    {
        // Arrange
        var genres = new List<Genre>().AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        var model = new CreateGenreModel { Name = "Test Genre" };
        var genre = new Genre { Name = "Test Genre" };
        _mockMapper.Setup(x => x.Map<Genre>(model)).Returns(genre);

        _command.Model = model;

        // Act
        _command.Handle();

        // Assert
        _mockContext.Verify(x => x.Genres.Add(It.IsAny<Genre>()), Times.Once);
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Handle_WhenGenreNameExists_ShouldThrowException()
    {
        // Arrange
        var existingGenre = new Genre { Name = "Test Genre" };
        var genres = new List<Genre> { existingGenre }.AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        var model = new CreateGenreModel { Name = "Test Genre" };
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