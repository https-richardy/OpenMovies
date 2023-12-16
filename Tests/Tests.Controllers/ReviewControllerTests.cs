namespace OpenMovies.Controllers.Tests;

public class ReviewControllerTests
{
    private readonly ReviewController _controller;
    private readonly Fixture _fixture;
    private readonly Mock<IReviewService> _reviewService;
    private readonly Mock<IMovieService> _movieService;
    private readonly Mock<IUserService> _userService;

    public ReviewControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _reviewService = new Mock<IReviewService>();
        _movieService = new Mock<IMovieService>();
        _userService = new Mock<IUserService>();

        _controller = new ReviewController(
            _reviewService.Object,
            _userService.Object,
            _movieService.Object);

        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
    }

    [Fact]
    public async Task GetReviewByMovieId_WithValidMovieId_ReturnsOkResult()
    {
        var movieId = 1;
        var reviews = _fixture.CreateMany<Review>(5).ToList();

        _reviewService.Setup(service => service.GetReviewsByMovieAsync(movieId))
            .ReturnsAsync(reviews);

        var result = await _controller.GetReviewByMovieId(movieId);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var returnedReviews = Assert.IsType<List<Review>>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(reviews, returnedReviews);
    }

    [Fact]
    public async Task GetReviewByMovieId_WithInvalidMovieId_ReturnsNotFoundResult()
    {
        var invalidMovieId = -1;

        _reviewService.Setup(service => service.GetReviewsByMovieAsync(invalidMovieId))
            .ThrowsAsync(new InvalidOperationException("Movie not found"));

        var result = await _controller.GetReviewByMovieId(invalidMovieId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResult()
    {
        var movieId = 1;
        var reviewDTO = _fixture.Create<ReviewDTO>();

        var movie = _fixture.Create<Movie>();
        var user = _fixture.Create<IdentityUser>();

        _movieService.Setup(service => service.GetMovieById(movieId))
            .ReturnsAsync(movie);

        _userService.Setup(service => service.GetUserAsync(_controller.User))
            .ReturnsAsync(user);

        var result = await _controller.Create(reviewDTO, movieId);

        var actionResult = Assert.IsType<StatusCodeResult>(result);

        Assert.Equal(201, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidData_ValidationException_ReturnsBadRequestResult()
    {
        var movieId = 1;

        var reviewDTO = _fixture.Create<ReviewDTO>();
        var movie = _fixture.Create<Movie>();
        var user = _fixture.Create<IdentityUser>();

        _movieService.Setup(service => service.GetMovieById(movieId))
            .ReturnsAsync(movie);

        _userService.Setup(service => service.GetUserAsync(_controller.User))
            .ReturnsAsync(user);

        _reviewService.Setup(service => service.AddReviewAsync(It.IsAny<Review>()))
            .Throws(new ValidationException("Validation failed"));

        var result = await _controller.Create(reviewDTO, movieId);
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WithValidReviewIdAndMatchingUser_ReturnsNoContent()
    {
        var reviewId = 1;
        var user = _fixture.Create<IdentityUser>();
        var existingReview = _fixture.Build<Review>().With(r => r.User, user).Create();

        _userService.Setup(service => service.GetUserAsync(_controller.User)).ReturnsAsync(user);
        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId)).ReturnsAsync(existingReview);

        var result = await _controller.Delete(reviewId);

        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);

        _reviewService.Verify(service => service.DeleteReviewAsync(existingReview), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidReviewIdAndMismatchedUser_ReturnsForbid()
    {
        var reviewId = 1;
        var user = _fixture.Create<IdentityUser>();
        var otherUser = _fixture.Create<IdentityUser>();
        var existingReview = _fixture.Build<Review>().With(r => r.User, otherUser).Create();

        _userService.Setup(service => service.GetUserAsync(_controller.User)).ReturnsAsync(user);
        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId)).ReturnsAsync(existingReview);

        var result = await _controller.Delete(reviewId);

        Assert.IsType<ForbidResult>(result);
        _reviewService.Verify(service => service.DeleteReviewAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WithInvalidReviewId_ReturnsNotFound()
    {
        var reviewId = -1;

        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId))
            .ThrowsAsync(new InvalidOperationException("Review not found"));

        var result = await _controller.Delete(reviewId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Update_WithValidReviewIdAndMatchingUser_ReturnsNoContent()
    {
        var reviewId = 1;
        var user = _fixture.Create<IdentityUser>();
        var existingReview = _fixture.Build<Review>().With(r => r.User, user).Create();
        var reviewDTO = _fixture.Create<ReviewDTO>();

        _userService.Setup(service => service.GetUserAsync(_controller.User)).ReturnsAsync(user);
        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId)).ReturnsAsync(existingReview);

        var result = await _controller.Update(reviewId, reviewDTO);

        var actionResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, actionResult.StatusCode);

        _reviewService.Verify(service => service.UpdateReviewAsync(It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task Update_WithValidReviewIdAndMismatchedUser_ReturnsForbid()
    {
        var reviewId = 1;
        var user = _fixture.Create<IdentityUser>();
        var otherUser = _fixture.Create<IdentityUser>();
        var existingReview = _fixture.Build<Review>().With(r => r.User, otherUser).Create();
        var reviewDTO = _fixture.Create<ReviewDTO>();

        _userService.Setup(service => service.GetUserAsync(_controller.User)).ReturnsAsync(user);
        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId)).ReturnsAsync(existingReview);

        var result = await _controller.Update(reviewId, reviewDTO);

        Assert.IsType<ForbidResult>(result);
        _reviewService.Verify(service => service.UpdateReviewAsync(It.IsAny<Review>()), Times.Never);
    }

    [Fact]
    public async Task Update_WithInvalidReviewId_ReturnsNotFound()
    {
        var reviewId = -1;
        var reviewDTO = _fixture.Create<ReviewDTO>();

        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId))
            .ThrowsAsync(new InvalidOperationException("Review not found"));

        var result = await _controller.Update(reviewId, reviewDTO);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Update_WithValidationException_ReturnsBadRequest()
    {
        var reviewId = 1;
        var user = _fixture.Create<IdentityUser>();
        var existingReview = _fixture.Build<Review>().With(r => r.User, user).Create();
        var reviewDTO = _fixture.Create<ReviewDTO>();

        _userService.Setup(service => service.GetUserAsync(_controller.User)).ReturnsAsync(user);
        _reviewService.Setup(service => service.GetReviewAsync(r => r.Id == reviewId)).ReturnsAsync(existingReview);
        _reviewService.Setup(service => service.UpdateReviewAsync(It.IsAny<Review>()))
            .Throws(new ValidationException("Validation failed"));

        var result = await _controller.Update(reviewId, reviewDTO);

        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, actionResult.StatusCode);
    }
}