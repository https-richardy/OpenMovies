using System.Linq.Expressions;
using OpenMovies.Data;
using OpenMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenMovies.Repositories;

public class DirectorRepository : IDirectorRepository
{
    private readonly AppDbContext _dbContext;

    public DirectorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Director entity)
    {
        await _dbContext.Directors.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Director entity)
    {
        _dbContext.Directors.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Director>> GetAllAsync()
    {
        return await _dbContext.Directors.ToListAsync();
    }

    public async Task<IEnumerable<Director>> GetAllAsync(Expression<Func<Director, bool>> predicate)
    {
        return await _dbContext.Directors.Where(predicate).ToListAsync();
    }

    # pragma warning restore
    public async Task<Director> GetAsync(Expression<Func<Director, bool>> predicate)
    {
        # pragma warning disable CS8603
        return await _dbContext.Directors.FirstOrDefaultAsync(predicate);
    }

    public async Task<Director> GetByIdAsync(int id)
    {
        return await _dbContext.Directors.FindAsync(id);
    }

    # pragma warning restore
    public async Task UpdateAsync(Director entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}