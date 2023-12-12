using OpenMovies.Models;

namespace OpenMovies.Services;

public interface IReviewService
{
    Task<Review> GetReviewAsync(Review review);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(Review review);
    Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId);
}