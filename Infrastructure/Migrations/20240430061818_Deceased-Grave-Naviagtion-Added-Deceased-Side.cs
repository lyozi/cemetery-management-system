using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeceasedGraveNaviagtionAddedDeceasedSide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons");

            migrationBuilder.RenameColumn(
                name: "GraveId",
                table: "DeceasedItems",
                newName: "graveId");

            migrationBuilder.RenameIndex(
                name: "IX_DeceasedItems_GraveId",
                table: "DeceasedItems",
                newName: "IX_DeceasedItems_graveId");

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "GraveUIPolygons",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_graveId",
                table: "DeceasedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons");

            migrationBuilder.RenameColumn(
                name: "graveId",
                table: "DeceasedItems",
                newName: "GraveId");

            migrationBuilder.RenameIndex(
                name: "IX_DeceasedItems_graveId",
                table: "DeceasedItems",
                newName: "IX_DeceasedItems_GraveId");

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "GraveUIPolygons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "DeceasedItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
