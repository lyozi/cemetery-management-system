using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeleteGravePolygon : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_MessageItems_DeceasedItems_DeceasedId",
                table: "MessageItems");

            migrationBuilder.AlterColumn<long>(
                name: "DeceasedId",
                table: "MessageItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "GraveUIPolygons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageItems_DeceasedItems_DeceasedId",
                table: "MessageItems",
                column: "DeceasedId",
                principalTable: "DeceasedItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GraveUIPolygons_GraveItems_GraveId",
                table: "GraveUIPolygons");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageItems_DeceasedItems_DeceasedId",
                table: "MessageItems");

            migrationBuilder.AlterColumn<long>(
                name: "DeceasedId",
                table: "MessageItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "GraveUIPolygons",
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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageItems_DeceasedItems_DeceasedId",
                table: "MessageItems",
                column: "DeceasedId",
                principalTable: "DeceasedItems",
                principalColumn: "Id");
        }
    }
}
