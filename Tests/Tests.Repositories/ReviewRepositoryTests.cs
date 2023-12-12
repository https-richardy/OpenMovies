# pragma warning disable CS8618, CS8602

namespace OpenMovies.Repositories.Tests;

public class ReviewRepositoryTests : IAsyncLifetime
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _dbContext;
    private Fixture _fixture;


    public async Task InitializeAsync()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(_options);
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldAddReviewInDatabase()
    {
        var reviewRepository = new ReviewRepository(_dbContext);
        var review = _fixture.Create<Review>();

        await reviewRepository.AddAsync(review);

        var result = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Movie.Id == review.Movie.Id);

        Assert.NotNull(result);
        Assert.Equal(review.Movie.Id, result.Movie.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteReviewFromDatabase()
    {
        var reviewRepository = new ReviewRepository(_dbContext);
        var review = _fixture.Create<Review>();

        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();

        await reviewRepository.DeleteAsync(review);
        var result = await _dbContext.Reviews.FindAsync(review.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldRetrieveReviewBaseOnPredicate()
    {
        var reviewRepository = new ReviewRepository(_dbContext);
        var review = _fixture.Create<Review>();

        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();

        var result = await reviewRepository.GetAsync(r => r.Id == review.Id);

        Assert.NotNull(result);
        Assert.Equal(review.Id, result.Id);
    }

    [Fact]
    public async Task GetReviewsByMovieAsync_ShouldReturnReviewsFromMovie()
    {
        var reviewRepository = new ReviewRepository(_dbContext);
        var movie = _fixture.Create<Movie>();
        var reviews = _fixture.Build<Review>().With(r => r.Movie, movie).CreateMany(3);

        await _dbContext.Reviews.AddRangeAsync(reviews);
        await _dbContext.SaveChangesAsync();

        var result = await reviewRepository.GetReviewsByMovieAsync(movie.Id);

        Assert.NotNull(result);
        Assert.Equal(reviews.Count(), result.Count());

        foreach (var review in reviews)
        {
            Assert.Equal(review.Movie.Id, movie.Id);
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateReview()
    {
        var reviewRepository = new ReviewRepository(_dbContext);
        var review = _fixture.Create<Review>();

        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();

        review.Comment = "new comment";
        await reviewRepository.UpdateAsync(review);

        var result = await _dbContext.Reviews.FindAsync(review.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Comment, review.Comment);
    }
}