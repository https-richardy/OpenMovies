using System.Linq.Expressions;
using OpenMovies.Models;

namespace OpenMovies.Repositories;

public interface IReviewRepository
{
    Task<Review> GetAsync(Expression<Func<Review, bool>> predicate);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
    Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId);
}