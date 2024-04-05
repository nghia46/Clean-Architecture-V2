// GenericRepository implementation
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanIsClean.Infrastructure.Repositories;
public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly SqliteDatabaseContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(SqliteDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id) ?? throw new ArgumentNullException(nameof(id));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
