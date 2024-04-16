// GenericRepository implementation
using System.Linq.Expressions;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanIsClean.Infrastructure.Repositories;
public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly SqliteDatabaseContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    private static readonly char[] separator = [','];

    public GenericRepository(SqliteDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (separator, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid? id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
