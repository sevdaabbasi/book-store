using Moq;
using WebApi.DbOperations;
using WebApi.GenreOperations.DeleteGenre;
using Xunit;

namespace WebApi.UnitTests.GenreTests;

public class DeleteGenreCommandTests
{
    private readonly Mock<BookStoreDbContext> _mockContext;
    private readonly DeleteGenreCommand _command;

    public DeleteGenreCommandTests()
    {
        _mockContext = new Mock<BookStoreDbContext>();
        _command = new DeleteGenreCommand(_mockContext.Object);
    }

    [Fact]
    public void Handle_WhenGenreExists_ShouldDeleteGenre()
    {
        // Arrange
        var genre = new Genre { Id = 1, Name = "Test Genre" };
        var genres = new List<Genre> { genre }.AsQueryable();
        var mockDbSet = MockDbSet(genres);
        _mockContext.Setup(x => x.Genres).Returns(mockDbSet.Object);

        _command.GenreId = 1;

        // Act
        _command.Handle();

        // Assert
        _mockContext.Verify(x => x.Genres.Remove(It.IsAny<Genre>()), Times.Once);
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