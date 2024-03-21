using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OpenMovies.Data;
using OpenMovies.Models;

namespace OpenMovies.Infrastructure.Data.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _dbContext;

    public ReviewRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Review review)
    {
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Review review)
    {
        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();
    }

    # pragma warning disable CS8603
    public async Task<Review> GetAsync(Expression<Func<Review, bool>> predicate)
    {
        return await _dbContext.Reviews.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId)
    {
        return await _dbContext.Reviews.Where(r => r.Movie.Id == movieId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        _dbContext.Entry(review).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}