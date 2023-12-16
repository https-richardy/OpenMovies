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

        _userService.Setup(um => um.GetUserAsync(_controller.User))
            .ReturnsAsync(user);

        var result = await _controller.Create(reviewDTO, movieId);

        var actionResult = Assert.IsType<StatusCodeResult>(result);

        Assert.Equal(201, actionResult.StatusCode);
    }

}