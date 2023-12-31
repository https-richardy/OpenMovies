using System.Linq.Expressions;
using AutoFixture;
using FluentValidation.Results;

namespace OpenMovies.Services.Tests;
public class MovieServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IDirectorRepository> _directorRepositoryMock;
    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _movieRepositoryMock = new Mock<IMovieRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _directorRepositoryMock = new Mock<IDirectorRepository>();

        _movieService = new MovieService(
            _movieRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _directorRepositoryMock.Object);
    }

    [Fact]
    public async Task GetMovieById_ValidId_ReturnsMovieWithEmbeddedTrailers()
    {
        var movie = new Movie("Movie 1", DateTime.Now, "Synopsis", It.IsAny<Director>(), It.IsAny<Category>());
        var trailers = new List<Trailer>()
        {
            new Trailer(TrailerType.Official, TrailerPlataform.Youtube, "https://youtube.com/watch?v=example", movie)
        };

        movie.Trailers = trailers;

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);

        var result = await _movieService.GetMovieById(1);

        Assert.NotNull(result);

        # pragma warning disable CS8604, xUnit2000
        Assert.Single(result.Trailers);
        Assert.Equal(movie.Trailers.First().Link, "https://www.youtube.com/embed/example");
    }

    [Fact]
    public async Task GetAllMovies_ReturnsAllMovies()
    {
        var expectedMovies = _fixture.CreateMany<Movie>().ToList();

        _movieRepositoryMock.Setup(repo => repo.GetAllMoviesAsync())
            .ReturnsAsync(expectedMovies);

        var result = await _movieService.GetAllMovies();

        Assert.Equal(expectedMovies, result);
    }

    [Fact]
    public async Task GetMovieById_InvalidId_ReturnsNull()
    {
        var invalidMovieId = -1;

        # pragma warning disable CS8620, CS8600
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.GetMovieById(invalidMovieId));
    }

    [Fact]
    public async Task CreateMovie_SuccessfulCreation()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var movie = new Movie("Valid Title", DateTime.Now, validSynopsis, director, category);
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        _directorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync(_fixture.Create<Director>());

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(_fixture.Create<Category>());

        await _movieService.CreateMovie(movie);
        _movieRepositoryMock.Verify(repo => repo.AddAsync(movie), Times.Once);
    }

    [Fact]
    public async Task CreateMovie_ValidationFailure()
    {
        var movie = _fixture.Create<Movie>();
        var validationErrors = _fixture.CreateMany<ValidationFailure>().ToList();
        var validationException = new ValidationException("Validation failed", validationErrors);

        var validationMock = new Mock<IValidator<Movie>>();
        validationMock.Setup(v => v.ValidateAsync(movie, default))
            .ReturnsAsync(new ValidationResult(validationErrors));

        await Assert.ThrowsAsync<ValidationException>(async () => await _movieService.CreateMovie(movie));

    }

    [Fact]
    public async Task CreateMovie_DuplicateTitleFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);
        var newMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(newMovie));
    }

    [Fact]
    public async Task CreateMovie_DirectorNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        _directorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync((Director)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(movie));
    }

    [Fact]
    public async Task CreateMovie_CategoryNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        _directorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync(_fixture.Create<Director>());

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Category)null);

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _movieService.CreateMovie(movie));
    }

   [Fact]
    public async Task DeleteMovie_SuccessfulDeletion()
    {
        var existingMovie = _fixture.Create<Movie>();
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(existingMovie);

        await _movieService.DeleteMovie(1);
        _movieRepositoryMock.Verify(repo => repo.DeleteAsync(existingMovie), Times.Once);
    }

    [Fact]
    public async Task DeleteMovie_InvalidId_ThrowsException()
    {
        var movieId = -1;
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.DeleteMovie(movieId));
    }

    [Fact]
    public async Task UpdateMovie_SuccessfulUpdate()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var movie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);

        var updatedMovie = movie;
        updatedMovie.Title = "updated";

        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync(movie);
        _movieRepositoryMock.Setup(repo => repo.UpdateAsync(movie))
            .Returns(Task.CompletedTask);

        _directorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync(_fixture.Create<Director>());
        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(_fixture.Create<Category>());

        await _movieService.UpdateMovie(updatedMovie);
        _movieRepositoryMock.Verify(repo => repo.UpdateAsync(movie), Times.Once);
    }

    [Fact]
    public async Task UpdateMovie_ValidationFailure()
    {
        var invalidMovie = _fixture.Create<Movie>();
        invalidMovie.Title = "";

        await Assert.ThrowsAsync<ValidationException>(() => _movieService.UpdateMovie(invalidMovie));
    }

    [Fact]
    public async Task UpdateMovie_MovieNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var nonExistingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);
        _movieRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(nonExistingMovie));
    }

    [Fact]
    public async Task UpdateMovie_DirectorNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var existingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);
        var updatedMovie = existingMovie;

        _directorRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync((Director)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(updatedMovie));
    }

    [Fact]
    public async Task UpdateMovie_CategoryNotFoundFailure()
    {
        var validSynopsis = new string('x', 60);
        var category = new Category { Id = 1, Name = "Action" };
        var director = new Director { Id = 1, FirstName = "John", LastName = "Doe" };

        var existingMovie = new Movie("Existing Movie", DateTime.Now, validSynopsis, director, category);
        var updatedMovie = existingMovie;

        _categoryRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync((Category)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _movieService.UpdateMovie(updatedMovie));
    }

    [Fact]
    public void CreateTrailers_SuccessfulCreation()
    {
        var movie = _fixture.Create<Movie>();
        var trailerDTOs = _fixture.CreateMany<TrailerDTO>().ToList();

        var trailers = _movieService.CreateTrailers(trailerDTOs, movie);

        Assert.NotNull(trailers);
        Assert.Equal(trailerDTOs.Count, trailers.Count);

        for (int i = 0; i < trailerDTOs.Count; i++)
        {
            Assert.Equal(trailerDTOs[i].Type, trailers[i].Type);
            Assert.Equal(trailerDTOs[i].Plataform, trailers[i].Plataform);
            Assert.Equal(trailerDTOs[i].Link, trailers[i].Link);
            Assert.Equal(movie, trailers[i].Movie);
        }
    }

    [Fact]
    public async Task AddTrailersToMovie_SuccessfulAddition()
    {
        var movie = _fixture.Create<Movie>();
        var trailers = _fixture.CreateMany<Trailer>().ToList();

        await _movieService.AddTrailersToMovie(movie, trailers);

        _movieRepositoryMock.Verify(repo => repo.AddTrailersAsync(movie, trailers), Times.Once);
        Assert.Equal(trailers, movie.Trailers);
    }
}