using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageUrlFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "GraveUIPolygons");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "GraveItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "GraveItems");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "GraveUIPolygons",
                type: "text",
                nullable: true);
        }
    }
}
