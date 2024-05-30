using Microsoft.EntityFrameworkCore;
using WarrAttachmentManagementService.Application.Interfaces.Persistence;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Repositories;

internal class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : EntityBase
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _dbSet
            .ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity) =>
        _dbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) =>
        _dbSet.AddRange(entities);

    public void Update(TEntity entity) =>
        _dbContext.Entry(entity).State = EntityState.Modified;

    public virtual void Delete(TEntity entity) =>
        _dbSet.Remove(entity);

    public void Detach(TEntity entity) =>
        _dbContext.Entry(entity).State = EntityState.Detached;
}
