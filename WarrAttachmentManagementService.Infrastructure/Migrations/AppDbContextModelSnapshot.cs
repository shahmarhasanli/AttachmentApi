﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarrAttachmentManagementService.Infrastructure.Persistence;

#nullable disable

namespace WarrAttachmentManagementService.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.LineItem", b =>
                {
                    b.Property<Guid>("LineItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LineItemID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool?>("AgentVerified")
                        .HasColumnType("bit");

                    b.Property<bool?>("AutoCloseRomaster")
                        .HasColumnType("bit")
                        .HasColumnName("AutoCloseROMaster");

                    b.Property<decimal?>("AutoSalesTaxAmount")
                        .HasColumnType("numeric(9,2)");

                    b.Property<string>("AutoSalesTaxDmscode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("AutoSalesTaxDMSCode");

                    b.Property<bool>("AutoSalesTaxIncluded")
                        .HasColumnType("bit");

                    b.Property<decimal?>("BillingTime")
                        .HasColumnType("numeric(9,2)");

                    b.Property<string>("Cause")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClaimTypeEnum")
                        .HasColumnType("int");

                    b.Property<string>("Complaint")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("DmslineStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("DMSLineStatus");

                    b.Property<string>("DmstypeCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("DMSTypeCode");

                    b.Property<decimal?>("LaborCharges")
                        .HasColumnType("numeric(9,2)");

                    b.Property<decimal?>("LaborRate")
                        .HasColumnType("numeric(9,2)");

                    b.Property<int>("LineItemStatusEnum")
                        .HasColumnType("int");

                    b.Property<string>("LineNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LineUpdatedByWarr")
                        .HasColumnType("bit");

                    b.Property<string>("OemdataField01")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField01");

                    b.Property<string>("OemdataField02")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField02");

                    b.Property<string>("OemdataField03")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField03");

                    b.Property<string>("OemdataField04")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField04");

                    b.Property<string>("OemdataField05")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField05");

                    b.Property<string>("OemdataField06")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField06");

                    b.Property<string>("OemdataField07")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField07");

                    b.Property<string>("OemdataField08")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField08");

                    b.Property<string>("OemdataField09")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField09");

                    b.Property<string>("OemdataField10")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField10");

                    b.Property<string>("OemdataField11")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("OEMDataField11");

                    b.Property<string>("OemdataField12")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("OEMDataField12");

                    b.Property<string>("OemdataField13")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("OEMDataField13");

                    b.Property<string>("OemdataField14")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("OEMDataField14");

                    b.Property<string>("OemdataField15")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField15");

                    b.Property<string>("OemdataField16")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField16");

                    b.Property<string>("OemdataField17")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField17");

                    b.Property<string>("OemdataField18")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField18");

                    b.Property<string>("OemdataField19")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField19");

                    b.Property<string>("OemdataField20")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField20");

                    b.Property<string>("OemdataField21")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField21");

                    b.Property<string>("OemdataField22")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("OEMDataField22");

                    b.Property<string>("OemdataField23")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField23");

                    b.Property<string>("OemdataField24")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField24");

                    b.Property<string>("OemdataField25")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField25");

                    b.Property<string>("OemdataField26")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField26");

                    b.Property<string>("OemdataField27")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField27");

                    b.Property<string>("OemdataField28")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField28");

                    b.Property<string>("OemdataField29")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField29");

                    b.Property<string>("OemdataField30")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("OEMDataField30");

                    b.Property<string>("OpCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OpCodeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OpCodeUpdateCounter")
                        .HasColumnType("int");

                    b.Property<Guid>("RepairOrderId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RepairOrderID");

                    b.Property<string>("SpecialInstructions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Story")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoryConfidenceLevel")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StoryDate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("StoryMileage")
                        .HasColumnType("numeric(18,2)");

                    b.Property<decimal?>("StraightTime")
                        .HasColumnType("numeric(9,2)");

                    b.Property<string>("TechName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TechNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("TemplateCatalogId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("TemplateCatalogID");

                    b.Property<int>("TimeStampConfidenceLevel")
                        .HasColumnType("int");

                    b.Property<string>("TransferLineNumber")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("UpdateIntegrationStatus")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("UpdateTempValue")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("WorkType")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("LineItemId");

                    b.HasIndex(new[] { "RepairOrderId", "Deleted", "WorkType" }, "IDX_LineItem_RepairOrderID");

                    b.ToTable("LineItem", null, t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.LineItemAttachment", b =>
                {
                    b.Property<Guid>("LineItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LineItemID");

                    b.Property<string>("AttachmentTypeCode")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("ApprovedForClaimSubmission")
                        .HasColumnType("bit");

                    b.Property<Guid>("CreatedByID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("OriginalName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("UpdatedByID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("LineItemId", "AttachmentTypeCode", "FileName");

                    b.ToTable("LineItemAttachment", (string)null);
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.LineItemAttachmentType", b =>
                {
                    b.Property<string>("AttachmentTypeCode")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("AttachmentTypeDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AttachmentTypeCode");

                    b.ToTable("LineItemAttachmentType", null, t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.OemAttachmentLimitation", b =>
                {
                    b.Property<Guid>("OemFamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AllowedExtensions")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("CreatedByID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxLength")
                        .HasColumnType("int");

                    b.Property<Guid?>("UpdatedByID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("OemFamilyId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("OemFamilyId"), false);

                    b.ToTable("OemAttachmentLimitation", (string)null);
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.RepairOrder", b =>
                {
                    b.Property<Guid>("RepairOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RepairOrderID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime?>("ClosedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CustomerID");

                    b.Property<decimal?>("CustomerTotal")
                        .HasColumnType("numeric(18,2)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Dmsrostatus")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("DMSROStatus");

                    b.Property<DateTime?>("DocReceivedDateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("DocReceivedUserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DocReceivedUserID");

                    b.Property<int>("DocStatusEnum")
                        .HasColumnType("int");

                    b.Property<bool>("HardCopyImageReceived")
                        .HasColumnType("bit");

                    b.Property<decimal?>("InternalTotal")
                        .HasColumnType("numeric(18,2)");

                    b.Property<decimal?>("Mileage")
                        .HasColumnType("numeric(18,2)");

                    b.Property<decimal?>("MileageOut")
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTime?>("OpenDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("PostClaimAuditCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("RepairOrderNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RepairOrderStatusEnum")
                        .HasColumnType("int");

                    b.Property<Guid>("RoofTopOemid")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RoofTopOEMID");

                    b.Property<string>("ServiceAdvisorName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ServiceAdvisorNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("VehicleID");

                    b.Property<decimal?>("WarrantyTotal")
                        .HasColumnType("numeric(18,2)");

                    b.HasKey("RepairOrderId");

                    b.ToTable("RepairOrder", (string)null);
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.RepairOrderAttachment", b =>
                {
                    b.Property<Guid>("RepairOrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttachmentTypeCode")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("ApprovedForClaimSubmission")
                        .HasColumnType("bit");

                    b.Property<Guid>("CreatedByID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateTime>("CreatedDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletionDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("OriginalName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("UpdatedByID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("RepairOrderId", "AttachmentTypeCode", "FileName");

                    b.ToTable("RepairOrderAttachment", (string)null);
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.LineItemAttachment", b =>
                {
                    b.HasOne("WarrAttachmentManagementService.Domain.Entities.LineItem", "LineItem")
                        .WithMany("LineItemAttachments")
                        .HasForeignKey("LineItemId")
                        .IsRequired()
                        .HasConstraintName("FK_LineItemAttachment_LineItem");

                    b.Navigation("LineItem");
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.RepairOrderAttachment", b =>
                {
                    b.HasOne("WarrAttachmentManagementService.Domain.Entities.RepairOrder", "RepairOrder")
                        .WithMany("RepairOrderAttachments")
                        .HasForeignKey("RepairOrderId")
                        .IsRequired()
                        .HasConstraintName("FK_RepairOrderAttachment_RepairOrder");

                    b.Navigation("RepairOrder");
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.LineItem", b =>
                {
                    b.Navigation("LineItemAttachments");
                });

            modelBuilder.Entity("WarrAttachmentManagementService.Domain.Entities.RepairOrder", b =>
                {
                    b.Navigation("RepairOrderAttachments");
                });
#pragma warning restore 612, 618
        }
    }
}
