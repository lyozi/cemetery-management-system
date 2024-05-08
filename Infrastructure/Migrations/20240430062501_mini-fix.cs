using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minifix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems");

            migrationBuilder.AlterColumn<long>(
                name: "graveId",
                table: "DeceasedItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems",
                column: "graveId",
                principalTable: "GraveItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems");

            migrationBuilder.AlterColumn<long>(
                name: "graveId",
                table: "DeceasedItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems",
                column: "graveId",
                principalTable: "GraveItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
