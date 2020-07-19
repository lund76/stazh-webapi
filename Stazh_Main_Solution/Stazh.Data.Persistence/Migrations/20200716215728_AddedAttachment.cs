using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Stazh.Data.EFCore.Migrations
{
    public partial class AddedAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deleted = table.Column<bool>(nullable: false),
                    UniqueAttachmentName = table.Column<string>(nullable: true),
                    OriginalFileName = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<byte[]>(nullable: true),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ItemId",
                table: "Attachment",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_UniqueAttachmentName",
                table: "Attachment",
                column: "UniqueAttachmentName",
                unique: true,
                filter: "[UniqueAttachmentName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");
        }
    }
}
