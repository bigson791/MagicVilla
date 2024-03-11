using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    VillId = table.Column<int>(type: "int", nullable: false),
                    DetallesEspeciales = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillId",
                        column: x => x.VillId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 3, 10, 18, 8, 20, 226, DateTimeKind.Local).AddTicks(8827), new DateTime(2024, 3, 10, 18, 8, 20, 226, DateTimeKind.Local).AddTicks(8816) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 3, 10, 18, 8, 20, 226, DateTimeKind.Local).AddTicks(8830), new DateTime(2024, 3, 10, 18, 8, 20, 226, DateTimeKind.Local).AddTicks(8830) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillId",
                table: "NumeroVillas",
                column: "VillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5593), new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5579) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5596), new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5595) });
        }
    }
}
