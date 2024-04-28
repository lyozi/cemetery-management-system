using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GraveRowRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Row",
                table: "GraveItems");

            migrationBuilder.DropColumn(
                name: "GraveNumber",
                table: "DeceasedItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Row",
                table: "GraveItems",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "GraveNumber",
                table: "DeceasedItems",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
