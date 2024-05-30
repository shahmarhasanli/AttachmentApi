using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal class RepairOrderAttachmentConfiguration : AuditableEntityTypeConfiguration<RepairOrderAttachment>
{
    public override void Configure(EntityTypeBuilder<RepairOrderAttachment> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => new { e.RepairOrderId, e.AttachmentTypeCode, e.FileName });

        builder.ToTable("RepairOrderAttachment");

        builder.Property(e => e.AttachmentTypeCode).HasMaxLength(80);

        builder.Property(e => e.FileName).HasMaxLength(100);

        builder.Property(e => e.OriginalName).HasMaxLength(200);

        builder.Property(e => e.Notes).HasMaxLength(500);

        builder.HasOne(d => d.RepairOrder)
            .WithMany(p => p.RepairOrderAttachments)
            .HasForeignKey(d => d.RepairOrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RepairOrderAttachment_RepairOrder");
    }
}
