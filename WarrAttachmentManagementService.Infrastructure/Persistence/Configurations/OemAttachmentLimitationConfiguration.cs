namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class OemAttachmentLimitationConfiguration
    : AuditableEntityTypeConfiguration<OemAttachmentLimitation>
{
    public override void Configure(EntityTypeBuilder<OemAttachmentLimitation> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => e.OemFamilyId).IsClustered(false);

        builder.Property(e => e.OemFamilyId).ValueGeneratedNever();
        builder.Property(e => e.AllowedExtensions).HasMaxLength(500);

        builder.ToTable("OemAttachmentLimitation");
    }
}