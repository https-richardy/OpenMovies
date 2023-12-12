namespace OpenMovies.Services.Tests;

public class ReviewServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly ReviewService _reviewService;

    public ReviewServiceTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _reviewService = new ReviewService(_reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task AddReviewAsync_ValidReview_ShouldAddReview()
    {
        var validReview = _fixture.Create<Review>();
        await _reviewService.AddReviewAsync(validReview);

        _reviewRepositoryMock.Verify(repo => repo.AddAsync(validReview), Times.Once);
    }

    [Fact]
    # pragma warning disable CS8600, CS8604
    public async Task AddReviewAsync_NullReview_Should_ThrowArgumentNullException()
    {
        Review nullReview = null;
        await Assert.ThrowsAsync<ArgumentNullException>(() => _reviewService.AddReviewAsync(nullReview));
    }

    [Fact]
    public async Task DeleteReviewAsync_ExistingReview_ShouldDeleteReview()
    {
        var existingReview = _fixture.Create<Review>();
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync(existingReview);

        await _reviewService.DeleteReviewAsync(existingReview);
        _reviewRepositoryMock.Verify(repo => repo.DeleteAsync(existingReview), Times.Once);
    }

    [Fact]
    public async Task DeleteReviewAsync_NullReview_ShouldThrowInvalidOperationException()
    {
        Review nullReview = null;
        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.DeleteReviewAsync(nullReview));
    }

    [Fact]
    # pragma warning disable CS8620
    public async Task DeleteReviewAsync_NonExistingReview_ShouldThrowInvalidOperationException()
    {
        var nonExistingReview = _fixture.Create<Review>();
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync((Review)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.DeleteReviewAsync(nonExistingReview));
    }

    [Fact]
    public async Task GetReviewAsync_ExistingReviewWithValidPredicate_ShouldReturnReview()
    {
        var existingReview = _fixture.Create<Review>();
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync(existingReview);

        var result = await _reviewService.GetReviewAsync(r => r.Id == existingReview.Id);
        Assert.Equal(existingReview, result);
    }

    [Fact]
    public async Task GetReviewAsync_InvalidPredicate_ShouldThrowInvalidOperationException()
    {
        Expression<Func<Review, bool>> invalidPredicate = r => r.Id == 0;
        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.GetReviewAsync(invalidPredicate));
    }

    [Fact]
    public async Task GetReviewAsync_NonExistingReview_ShouldThrowInvalidOperationException()
    {
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync((Review)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.GetReviewAsync(r => r.Id == 1)); // Supondo que 1 seja um ID que não existe
    }

    [Fact]
    public async Task GetReviewsByMovieAsync_ExistingReviewsForMovie_ShouldReturnReviews()
    {
        var movieId = _fixture.Create<int>();
        var existingReviews = _fixture.CreateMany<Review>().ToList();
        _reviewRepositoryMock.Setup(repo => repo.GetReviewsByMovieAsync(movieId))
                            .ReturnsAsync(existingReviews);

        var result = await _reviewService.GetReviewsByMovieAsync(movieId);
        Assert.Equal(existingReviews, result);
    }

    [Fact]
    public async Task GetReviewsByMovieAsync_NonExistingMovie_ShouldThrowInvalidOperationException()
    {
        var movieId = _fixture.Create<int>();
        _reviewRepositoryMock.Setup(repo => repo.GetReviewsByMovieAsync(movieId))
                            .ReturnsAsync((IEnumerable<Review>)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.GetReviewsByMovieAsync(movieId));
    }

    [Fact]
    public async Task UpdateReviewAsync_ExistingReview_ShouldUpdateReview()
    {
        var existingReview = _fixture.Create<Review>();
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync(existingReview);

        await _reviewService.UpdateReviewAsync(existingReview);
        _reviewRepositoryMock.Verify(repo => repo.UpdateAsync(existingReview), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewAsync_NullReview_ShouldThrowInvalidOperationException()
    {
        Review nullReview = null;
        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.UpdateReviewAsync(nullReview));
    }

    [Fact]
    public async Task UpdateReviewAsync_NonExistingReview_ShouldThrowInvalidOperationException()
    {

        var nonExistingReview = _fixture.Create<Review>();
        _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                            .ReturnsAsync((Review)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _reviewService.UpdateReviewAsync(nonExistingReview));
    }
}