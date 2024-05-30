using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal abstract class AuditableEntityTypeConfiguration<TEntity>
    : EntityBaseConfiguration<TEntity>
    where TEntity : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.CreatedByID)
            .IsRequired()
            .HasDefaultValue(Guid.Empty);

        builder
            .Property(x => x.CreatedDateTime)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("getdate()");

        builder
            .HasQueryFilter(e => !e.Deleted);
    }
}
