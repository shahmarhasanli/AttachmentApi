using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal class RepairOrderConfiguration : EntityBaseConfiguration<RepairOrder>
{
    public override void Configure(EntityTypeBuilder<RepairOrder> builder)
    {
        base.Configure(builder);

        builder.ToTable("RepairOrder");

        builder.Property(e => e.RepairOrderId)
            .HasColumnName("RepairOrderID")
            .HasDefaultValueSql("(newid())");

        builder.Property(e => e.ClosedDate).HasColumnType("datetime");

        builder.Property(e => e.CreatedDateTime).HasColumnType("datetime");

        builder.Property(e => e.CustomerId).HasColumnName("CustomerID");

        builder.Property(e => e.CustomerTotal).HasColumnType("numeric(18, 2)");

        builder.Property(e => e.Dmsrostatus)
            .HasMaxLength(100)
            .HasColumnName("DMSROStatus");

        builder.Property(e => e.DocReceivedDateTime).HasColumnType("datetime");

        builder.Property(e => e.DocReceivedUserId).HasColumnName("DocReceivedUserID");

        builder.Property(e => e.InternalTotal).HasColumnType("numeric(18, 2)");

        builder.Property(e => e.Mileage).HasColumnType("numeric(18, 2)");

        builder.Property(e => e.MileageOut).HasColumnType("numeric(18, 2)");

        builder.Property(e => e.OpenDate).HasColumnType("datetime");

        builder.Property(e => e.RepairOrderNumber).HasMaxLength(50);

        builder.Property(e => e.RoofTopOemid).HasColumnName("RoofTopOEMID");

        builder.Property(e => e.ServiceAdvisorName).HasMaxLength(100);

        builder.Property(e => e.ServiceAdvisorNumber).HasMaxLength(50);

        builder.Property(e => e.UpdatedDateTime).HasColumnType("datetime");

        builder.Property(e => e.VehicleId).HasColumnName("VehicleID");

        builder.Property(e => e.WarrantyTotal).HasColumnType("numeric(18, 2)");

    }
}
