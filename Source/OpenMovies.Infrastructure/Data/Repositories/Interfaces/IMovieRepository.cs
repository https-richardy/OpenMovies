using System.Linq.Expressions;
using OpenMovies.Models;

namespace OpenMovies.Repositories;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<IEnumerable<Movie>> GetAllMoviesAsync(Expression<Func<Movie, bool>> predicate);

    Task<Movie> GetAsync(Expression<Func<Movie, bool>> predicate);
    Task<Movie> GetByIdAsync(int id);

    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);

    Task<IEnumerable<Movie>> SearchAsync(string? name = null, int? releaseYear = null, int? categoryId = null);
    Task AddTrailersAsync(Movie movie, List<Trailer> trailers);
}
