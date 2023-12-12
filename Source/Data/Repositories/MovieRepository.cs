using System.Linq.Expressions;
using OpenMovies.Data;
using OpenMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenMovies.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _dbContext;

    public MovieRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Movie entity)
    {
        await _dbContext.Movies.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Movie entity)
    {
        _dbContext.Movies.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _dbContext.Movies
        .Include(m => m.Director)
        .Include(m => m.Category)
        .Include(m => m.Trailers)
        .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(Expression<Func<Movie, bool>> predicate)
    {
        return await _dbContext.Movies.Where(predicate).ToListAsync();
    }

    # pragma warning restore
    public async Task<Movie> GetAsync(Expression<Func<Movie, bool>> predicate)
    {
        # pragma warning disable CS8603
        return await _dbContext.Movies
            .Include(m => m.Director)
            .Include(m => m.Category)
            .Include(m => m.Trailers)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<Movie> GetByIdAsync(int id)
    {
        return await _dbContext.Movies.FindAsync(id);
    }

    # pragma warning restore
    public async Task UpdateAsync(Movie entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    #pragma warning disable CS8602
    public async Task AddTrailersAsync(Movie movie, List<Trailer> trailers)
    {
        if (movie == null)
            throw new ArgumentNullException(nameof(movie));

        movie.Trailers.AddRange(trailers);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Movie>> SearchAsync(string? name = null, int? releaseYear = null, int? categoryId = null)
    {
        IQueryable<Movie> query = _dbContext.Movies.Include(m => m.Director).Include(m => m.Category).Include(m => m.Trailers);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(m => m.Title.ToLower().Contains(name.ToLower()));

        if (releaseYear.HasValue)
            query = query.Where(m => m.ReleaseDateOf.Year == releaseYear);

        if (categoryId.HasValue)
            query = query.Where(m => m.Category.Id == categoryId);

        return await query.ToListAsync();
    }
}