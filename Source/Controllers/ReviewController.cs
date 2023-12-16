using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenMovies.DTOs;
using OpenMovies.Models;
using OpenMovies.Services;

namespace OpenMovies.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;

    public ReviewController(
    IReviewService reviewService,
    IUserService userService,
    IMovieService movieService)
    {
        _reviewService = reviewService;
        _userService = userService;
        _movieService = movieService;
    }

    [AllowAnonymous]
    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetReviewByMovieId(int movieId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByMovieAsync(movieId);
            return Ok(reviews);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("{movieId}")]
    public async Task<IActionResult> Create([FromBody] ReviewDTO data, int movieId)
    {
        try
        {
            var movie = await _movieService.GetMovieById(movieId);
            var user = await _userService.GetUserAsync(User);

            var review = new Review(data.Liked, data.Comment, movie, user, data.Classification, data.ContainsSpoiler);
            await _reviewService.AddReviewAsync(review);

            return StatusCode(201);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}