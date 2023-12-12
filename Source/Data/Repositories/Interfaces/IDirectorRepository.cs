using System.Linq.Expressions;
using OpenMovies.Models;

namespace OpenMovies.Repositories;

public interface IDirectorRepository
{
    Task<IEnumerable<Director>> GetAllAsync();
    Task<IEnumerable<Director>> GetAllAsync(Expression<Func<Director, bool>> predicate);

    Task<Director> GetAsync(Expression<Func<Director, bool>> predicate);
    Task<Director> GetByIdAsync(int id);
    Task AddAsync(Director director);
    Task UpdateAsync(Director director);
    Task DeleteAsync(Director director);
}