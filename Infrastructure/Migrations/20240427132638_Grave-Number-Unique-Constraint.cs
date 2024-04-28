using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GraveNumberUniqueConstraint : Migration
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
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Number",
                table: "GraveItems",
                column: "Number",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems",
                column: "GraveId",
                principalTable: "GraveItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeceasedItems_GraveItems_GraveId",
                table: "DeceasedItems");

            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Number",
                table: "GraveItems");

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
        }
    }
}
