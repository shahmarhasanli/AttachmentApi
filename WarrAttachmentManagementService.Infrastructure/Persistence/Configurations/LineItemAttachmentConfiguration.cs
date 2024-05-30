using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal class LineItemAttachmentConfiguration : AuditableEntityTypeConfiguration<LineItemAttachment>
{
    public override void Configure(EntityTypeBuilder<LineItemAttachment> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => new { e.LineItemId, e.AttachmentTypeCode, e.FileName });

        builder.ToTable("LineItemAttachment");

        builder.Property(e => e.LineItemId).HasColumnName("LineItemID");

        builder.Property(e => e.AttachmentTypeCode).HasMaxLength(80);

        builder.Property(e => e.FileName).HasMaxLength(100);

        builder.Property(e => e.OriginalName).HasMaxLength(200);

        builder.Property(e => e.Notes).HasMaxLength(500);

        builder.HasOne(d => d.LineItem)
            .WithMany(p => p.LineItemAttachments)
            .HasForeignKey(d => d.LineItemId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_LineItemAttachment_LineItem");
    }
}
