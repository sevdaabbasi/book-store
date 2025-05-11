using Moq;
using WebApi.DbOperations;
using WebApi.GenreOperations.UpdateGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class UpdateGenreCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly UpdateGenreCommand _command;

    public UpdateGenreCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _command = new UpdateGenreCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_WhenGenreExists_ShouldUpdateGenre()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Old Genre" };
        var genres = new List<Genre> { genre }.AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        var model = new UpdateGenreModel { Name = "New Genre" };
        _command.GenreId = 1;
        _command.Model = model;

        // Act
        _command.Handle();

        // Assert
        Assert.Equal("New Genre", genre.Name);
        _mockContext.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Handle_WhenGenreDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var genres = new List<Genre>().AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        _command.GenreId = 1;
        _command.Model = new UpdateGenreModel();

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