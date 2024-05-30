using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarrAttachmentManagementService.Domain.Entities;

namespace WarrAttachmentManagementService.Infrastructure.Persistence.Configurations;

internal class LineItemConfiguration : EntityBaseConfiguration<LineItem>
{
    public override void Configure(EntityTypeBuilder<LineItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("LineItem", t => t.ExcludeFromMigrations());

        builder.HasIndex(e => new { e.RepairOrderId, e.Deleted, e.WorkType }, "IDX_LineItem_RepairOrderID");

        builder.Property(e => e.LineItemId)
            .HasColumnName("LineItemID")
            .HasDefaultValueSql("(newid())");

        builder.Property(e => e.AutoCloseRomaster).HasColumnName("AutoCloseROMaster");

        builder.Property(e => e.AutoSalesTaxAmount).HasColumnType("numeric(9, 2)");

        builder.Property(e => e.AutoSalesTaxDmscode)
            .HasMaxLength(50)
            .HasColumnName("AutoSalesTaxDMSCode");

        builder.Property(e => e.BillingTime).HasColumnType("numeric(9, 2)");

        builder.Property(e => e.CreatedDateTime).HasColumnType("datetime");

        builder.Property(e => e.DmslineStatus)
            .HasMaxLength(50)
            .HasColumnName("DMSLineStatus");

        builder.Property(e => e.DmstypeCode)
            .HasMaxLength(50)
            .HasColumnName("DMSTypeCode");

        builder.Property(e => e.LaborCharges).HasColumnType("numeric(9, 2)");

        builder.Property(e => e.LaborRate).HasColumnType("numeric(9, 2)");

        builder.Property(e => e.LineNumber).HasMaxLength(50);

        builder.Property(e => e.OemdataField01)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField01");

        builder.Property(e => e.OemdataField02)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField02");

        builder.Property(e => e.OemdataField03)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField03");

        builder.Property(e => e.OemdataField04)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField04");

        builder.Property(e => e.OemdataField05)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField05");

        builder.Property(e => e.OemdataField06)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField06");

        builder.Property(e => e.OemdataField07)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField07");

        builder.Property(e => e.OemdataField08)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField08");

        builder.Property(e => e.OemdataField09)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField09");

        builder.Property(e => e.OemdataField10)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField10");

        builder.Property(e => e.OemdataField11).HasColumnName("OEMDataField11");

        builder.Property(e => e.OemdataField12).HasColumnName("OEMDataField12");

        builder.Property(e => e.OemdataField13).HasColumnName("OEMDataField13");

        builder.Property(e => e.OemdataField14).HasColumnName("OEMDataField14");

        builder.Property(e => e.OemdataField15)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField15");

        builder.Property(e => e.OemdataField16)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField16");

        builder.Property(e => e.OemdataField17)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField17");

        builder.Property(e => e.OemdataField18)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField18");

        builder.Property(e => e.OemdataField19)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField19");

        builder.Property(e => e.OemdataField20)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField20");

        builder.Property(e => e.OemdataField21)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField21");

        builder.Property(e => e.OemdataField22).HasColumnName("OEMDataField22");

        builder.Property(e => e.OemdataField23)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField23");

        builder.Property(e => e.OemdataField24)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField24");

        builder.Property(e => e.OemdataField25)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField25");

        builder.Property(e => e.OemdataField26)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField26");

        builder.Property(e => e.OemdataField27)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField27");

        builder.Property(e => e.OemdataField28)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField28");

        builder.Property(e => e.OemdataField29)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField29");

        builder.Property(e => e.OemdataField30)
            .HasMaxLength(250)
            .HasColumnName("OEMDataField30");

        builder.Property(e => e.OpCode).HasMaxLength(50);

        builder.Property(e => e.RepairOrderId).HasColumnName("RepairOrderID");

        builder.Property(e => e.StoryDate).HasColumnType("datetime");

        builder.Property(e => e.StoryMileage).HasColumnType("numeric(18, 2)");

        builder.Property(e => e.StraightTime).HasColumnType("numeric(9, 2)");

        builder.Property(e => e.TechName).HasMaxLength(100);

        builder.Property(e => e.TechNumber).HasMaxLength(50);

        builder.Property(e => e.TemplateCatalogId).HasColumnName("TemplateCatalogID");

        builder.Property(e => e.TransferLineNumber).HasMaxLength(500);

        builder.Property(e => e.UpdateIntegrationStatus).HasMaxLength(250);

        builder.Property(e => e.UpdatedDateTime).HasColumnType("datetime");

        builder.Property(e => e.WorkType).HasMaxLength(1);


    }
}
