using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndexFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Table_Row_Parcel_Type",
                table: "GraveItems");

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Table_Row_Parcel",
                table: "GraveItems",
                columns: new[] { "Table", "Row", "Parcel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Table_Row_Parcel",
                table: "GraveItems");

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Table_Row_Parcel_Type",
                table: "GraveItems",
                columns: new[] { "Table", "Row", "Parcel", "Type" },
                unique: true);
        }
    }
}
