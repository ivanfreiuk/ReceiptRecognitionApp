using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReceiptRecognitionApp.Migrations
{
    public partial class EntetiesRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptImageId",
                table: "Receipts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReceiptImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OriginalImageName = table.Column<string>(nullable: true),
                    OriginalImage = table.Column<byte[]>(nullable: true),
                    ScannedImageName = table.Column<string>(nullable: true),
                    ScannedImage = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_ReceiptImageId",
                table: "Receipts",
                column: "ReceiptImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_ReceiptImages_ReceiptImageId",
                table: "Receipts",
                column: "ReceiptImageId",
                principalTable: "ReceiptImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_ReceiptImages_ReceiptImageId",
                table: "Receipts");

            migrationBuilder.DropTable(
                name: "ReceiptImages");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_ReceiptImageId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "ReceiptImageId",
                table: "Receipts");
        }
    }
}
