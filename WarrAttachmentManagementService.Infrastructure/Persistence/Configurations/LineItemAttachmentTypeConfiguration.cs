using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal class LineItemAttachmentTypeConfiguration : AuditableEntityTypeConfiguration<LineItemAttachmentType>
{
    public override void Configure(EntityTypeBuilder<LineItemAttachmentType> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => new { e.AttachmentTypeCode });

        builder.ToTable("LineItemAttachmentType", t => t.ExcludeFromMigrations());

        builder.Property(e => e.AttachmentTypeCode).HasMaxLength(80);

        builder.Property(e => e.AttachmentTypeDescription).HasMaxLength(100);

        builder.Ignore(e => e.Deleted);
        builder.Ignore(e => e.UpdatedDateTime);
        builder.Ignore(e => e.CreatedDateTime);
        builder.Ignore(e => e.DeletionDateTime);
        builder.Ignore(e => e.CreatedByID);
        builder.Ignore(e => e.UpdatedByID);
    }
}
