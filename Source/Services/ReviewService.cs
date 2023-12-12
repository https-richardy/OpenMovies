using System.Linq.Expressions;
using OpenMovies.Models;
using OpenMovies.Repositories;

namespace OpenMovies.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task AddReviewAsync(Review review)
    {
        if (review == null)
            throw new ArgumentNullException(nameof(review), "Review object cannot be null.");

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
        return reviews;
    }

    public async Task UpdateReviewAsync(Review review)
    {
        var existingReview = await _reviewRepository.GetAsync(r => r.Id == review.Id);
        if (existingReview == null)
            throw new InvalidOperationException("Review not found.");

        await _reviewRepository.UpdateAsync(review);
    }
}