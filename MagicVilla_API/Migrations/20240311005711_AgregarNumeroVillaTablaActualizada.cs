using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroVillaTablaActualizada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumeroVillas_Villas_VillId",
                table: "NumeroVillas");

            migrationBuilder.DropIndex(
                name: "IX_NumeroVillas_VillId",
                table: "NumeroVillas");

            migrationBuilder.DropColumn(
                name: "VillId",
                table: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 3, 10, 18, 57, 11, 749, DateTimeKind.Local).AddTicks(4642), new DateTime(2024, 3, 10, 18, 57, 11, 749, DateTimeKind.Local).AddTicks(4632) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2024, 3, 10, 18, 57, 11, 749, DateTimeKind.Local).AddTicks(4645), new DateTime(2024, 3, 10, 18, 57, 11, 749, DateTimeKind.Local).AddTicks(4645) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumeroVillas_Villas_VillaId",
                table: "NumeroVillas",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumeroVillas_Villas_VillaId",
                table: "NumeroVillas");

            migrationBuilder.DropIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas");

            migrationBuilder.AddColumn<int>(
                name: "VillId",
                table: "NumeroVillas",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_NumeroVillas_Villas_VillId",
                table: "NumeroVillas",
                column: "VillId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
