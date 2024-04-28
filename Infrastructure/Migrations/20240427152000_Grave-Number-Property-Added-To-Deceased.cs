using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GraveNumberPropertyAddedToDeceased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems");

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "DeceasedItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<short>(
                name: "GraveNumber",
                table: "DeceasedItems",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

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

            migrationBuilder.DropColumn(
                name: "GraveNumber",
                table: "DeceasedItems");

            migrationBuilder.AlterColumn<long>(
                name: "GraveId",
                table: "DeceasedItems",
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
        }
    }
}
