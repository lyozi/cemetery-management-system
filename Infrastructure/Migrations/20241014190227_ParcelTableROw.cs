using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ParcelTableROw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Number",
                table: "GraveItems");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "GraveItems",
                newName: "Row");

            migrationBuilder.AddColumn<short>(
                name: "Parcel",
                table: "GraveItems",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<char>(
                name: "Table",
                table: "GraveItems",
                type: "character(1)",
                nullable: false,
                defaultValue: 'Z');
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parcel",
                table: "GraveItems");

            migrationBuilder.DropColumn(
                name: "Table",
                table: "GraveItems");

            migrationBuilder.RenameColumn(
                name: "Row",
                table: "GraveItems",
                newName: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Number",
                table: "GraveItems",
                column: "Number",
                unique: true);
        }
    }
}
