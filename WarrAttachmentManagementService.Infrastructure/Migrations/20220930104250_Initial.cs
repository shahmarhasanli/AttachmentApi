using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarrAttachmentManagementService.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepairOrderAttachment",
                columns: table => new
                {
                    RepairOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentTypeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairOrderAttachment", x => new { x.RepairOrderId, x.AttachmentTypeCode, x.FileName });
                    table.ForeignKey(
                        name: "FK_RepairOrderAttachment_RepairOrder",
                        column: x => x.RepairOrderId,
                        principalTable: "RepairOrder",
                        principalColumn: "RepairOrderID");
                    table.ForeignKey(
                        name: "FK_RepairOrderAttachment_RepairOrderAttachmentType",
                        column: x => x.AttachmentTypeCode,
                        principalTable: "LineItemAttachmentType",
                        principalColumn: "AttachmentTypeCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairOrderAttachment_AttachmentTypeCode",
                table: "RepairOrderAttachment",
                column: "AttachmentTypeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairOrderAttachment");
        }
    }
}
