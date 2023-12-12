using System.Linq.Expressions;
using OpenMovies.Models;

namespace OpenMovies.Services;

public interface IReviewService
{
    Task<Review> GetReviewAsync(Expression<Func<Review, bool>> predicate);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(Review review);
    Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId);
}