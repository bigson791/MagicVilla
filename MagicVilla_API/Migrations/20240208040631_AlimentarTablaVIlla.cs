using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVIlla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5593), new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5579), "", 50.0, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la villa...", new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5596), new DateTime(2024, 2, 7, 22, 6, 31, 170, DateTimeKind.Local).AddTicks(5595), "", 40.0, "Premium Vista a la piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
