using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal abstract class EntityBaseConfiguration<TEntity>
        : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
{
    public virtual void Configure(
    EntityTypeBuilder<TEntity> builder)
    {
    }
}
