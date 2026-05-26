using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameParcelToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // GraveItems: drop the composite index that referenced the old char Table column,
            // drop that unused column, then rename Parcel (short) → Table (preserving data).
            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Table_Row_Parcel",
                table: "GraveItems");

            migrationBuilder.DropColumn(
                name: "Table",
                table: "GraveItems");

            migrationBuilder.RenameColumn(
                name: "Parcel",
                table: "GraveItems",
                newName: "Table");

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Table_Row",
                table: "GraveItems",
                columns: new[] { "Table", "Row" });

            // Parcels → Tables, ParcelPoints → TablePoints (data preserved).
            migrationBuilder.RenameTable(
                name: "Parcels",
                newName: "Tables");

            migrationBuilder.RenameTable(
                name: "ParcelPoints",
                newName: "TablePoints");

            migrationBuilder.RenameColumn(
                name: "ParcelId",
                table: "TablePoints",
                newName: "TableId");

            migrationBuilder.RenameIndex(
                name: "IX_ParcelPoints_ParcelId",
                table: "TablePoints",
                newName: "IX_TablePoints_TableId");

            // Postgres: rename PK/FK constraints to match new table/column names.
            migrationBuilder.Sql("ALTER TABLE \"Tables\" RENAME CONSTRAINT \"PK_Parcels\" TO \"PK_Tables\";");
            migrationBuilder.Sql("ALTER TABLE \"TablePoints\" RENAME CONSTRAINT \"PK_ParcelPoints\" TO \"PK_TablePoints\";");
            migrationBuilder.Sql("ALTER TABLE \"TablePoints\" RENAME CONSTRAINT \"FK_ParcelPoints_Parcels_ParcelId\" TO \"FK_TablePoints_Tables_TableId\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TablePoints\" RENAME CONSTRAINT \"FK_TablePoints_Tables_TableId\" TO \"FK_ParcelPoints_Parcels_ParcelId\";");
            migrationBuilder.Sql("ALTER TABLE \"TablePoints\" RENAME CONSTRAINT \"PK_TablePoints\" TO \"PK_ParcelPoints\";");
            migrationBuilder.Sql("ALTER TABLE \"Tables\" RENAME CONSTRAINT \"PK_Tables\" TO \"PK_Parcels\";");

            migrationBuilder.RenameIndex(
                name: "IX_TablePoints_TableId",
                table: "TablePoints",
                newName: "IX_ParcelPoints_ParcelId");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "TablePoints",
                newName: "ParcelId");

            migrationBuilder.RenameTable(
                name: "TablePoints",
                newName: "ParcelPoints");

            migrationBuilder.RenameTable(
                name: "Tables",
                newName: "Parcels");

            migrationBuilder.DropIndex(
                name: "IX_GraveItems_Table_Row",
                table: "GraveItems");

            migrationBuilder.RenameColumn(
                name: "Table",
                table: "GraveItems",
                newName: "Parcel");

            migrationBuilder.AddColumn<char>(
                name: "Table",
                table: "GraveItems",
                type: "character(1)",
                nullable: false,
                defaultValue: 'Z');

            migrationBuilder.CreateIndex(
                name: "IX_GraveItems_Table_Row_Parcel",
                table: "GraveItems",
                columns: new[] { "Table", "Row", "Parcel" });
        }
    }
}
