using System.Linq.Expressions;
using FluentValidation;
using OpenMovies.Models;
using OpenMovies.Repositories;

namespace OpenMovies.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IValidator<Review> _reviewValidator;

    public ReviewService(IReviewRepository reviewRepository, IValidator<Review> reviewValidator)
    {
        _reviewRepository = reviewRepository;
        _reviewValidator = reviewValidator;
    }

    public async Task AddReviewAsync(Review review)
    {
        if (review == null)
            throw new ArgumentNullException(nameof(review), "Review object cannot be null.");

        var validationResult = _reviewValidator.Validate(review);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _reviewRepository.AddAsync(review);
    }

    public async Task DeleteReviewAsync(Review review)
    {
        var retrievedReview = await _reviewRepository.GetAsync(r => r.Id == review.Id);
        if (retrievedReview == null)
            throw new InvalidOperationException("Review not found.");

        await _reviewRepository.DeleteAsync(review);
    }

    public async Task<Review> GetReviewAsync(Expression<Func<Review, bool>> predicate)
    {
        var retrievedReview = await _reviewRepository.GetAsync(predicate);
        if (retrievedReview == null)
            throw new InvalidOperationException("Review not found.");

        return retrievedReview;
    }

    public async Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId)
    {
        var reviews = await _reviewRepository.GetReviewsByMovieAsync(movieId);
        if (reviews == null)
            throw new InvalidOperationException("Movie not found.");

        return reviews;
    }

    public async Task UpdateReviewAsync(Review review)
    {
        var existingReview = await _reviewRepository.GetAsync(r => r.Id == review.Id);
        if (existingReview == null)
            throw new InvalidOperationException("Review not found.");

        existingReview.Liked = review.Liked;
        existingReview.Comment = review.Comment;
        existingReview.Classification = review.Classification;
        existingReview.ContainsSpoiler = review.ContainsSpoiler;


        var validationResult = _reviewValidator.Validate(existingReview);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _reviewRepository.UpdateAsync(review);
    }
}