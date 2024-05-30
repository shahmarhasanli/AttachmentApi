using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Application.Interfaces.Persistence;

public interface IRepositoryBase<TEntity>
        where TEntity : EntityBase
{
    Task<ICollection<TEntity>> GetAllAsync(
        CancellationToken cancellationToken);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    /*
    void SoftDelete(TEntity entity);

    void SoftDeleteRange(IEnumerable<TEntity> entities);
    */
    void Delete(TEntity entity);

    void Detach(TEntity entity);
}
