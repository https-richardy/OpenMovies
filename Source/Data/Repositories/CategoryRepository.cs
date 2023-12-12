using System.Linq.Expressions;
using OpenMovies.Data;
using OpenMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace OpenMovies.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Category entity)
    {
        await _dbContext.Categories.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        _dbContext.Categories.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _dbContext.Categories.Where(predicate).ToListAsync();
    }

    # pragma warning restore
    public async Task<Category> GetAsync(Expression<Func<Category, bool>> predicate)
    {
        # pragma warning disable CS8603
        return await _dbContext.Categories.FirstOrDefaultAsync(predicate);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _dbContext.Categories.FindAsync(id);
    }

    # pragma warning restore
    public async Task UpdateAsync(Category entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}