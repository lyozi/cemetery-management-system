using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems");

            migrationBuilder.RenameColumn(
                name: "graveId",
                table: "DeceasedItems",
                newName: "GraveId");

            migrationBuilder.RenameIndex(
                name: "IX_DeceasedItems_graveId",
                table: "DeceasedItems",
                newName: "IX_DeceasedItems_GraveId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems");

            migrationBuilder.RenameColumn(
                name: "GraveId",
                table: "DeceasedItems",
                newName: "graveId");

            migrationBuilder.RenameIndex(
                name: "IX_DeceasedItems_GraveId",
                table: "DeceasedItems",
                newName: "IX_DeceasedItems_graveId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems",
                column: "graveId",
                principalTable: "GraveItems",
                principalColumn: "Id");
        }
    }
}
