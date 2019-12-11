using Microsoft.EntityFrameworkCore.Migrations;

namespace ReceiptRecognitionApp.Migrations
{
    public partial class AddFieldsToReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptDate",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiptTotal",
                table: "Receipts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Receipts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptDate",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "ReceiptTotal",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Receipts");
        }
    }
}
