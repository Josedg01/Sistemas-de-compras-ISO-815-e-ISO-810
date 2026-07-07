using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "Id", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, 1, "Compras" },
                    { 2, 1, "Contabilidad" },
                    { 3, 1, "Almacen" }
                });

            migrationBuilder.InsertData(
                table: "Proveedores",
                columns: new[] { "Id", "CedulaRnc", "Estado", "NombreComercial" },
                values: new object[,]
                {
                    { 1, "101000001", 1, "Suplidora Nacional SRL" },
                    { 2, "101000002", 1, "Distribuidora Caribe SRL" }
                });

            migrationBuilder.InsertData(
                table: "UnidadesMedida",
                columns: new[] { "Id", "Descripcion", "Estado" },
                values: new object[,]
                {
                    { 1, "Unidad", 1 },
                    { 2, "Caja", 1 },
                    { 3, "Libra", 1 }
                });

            migrationBuilder.InsertData(
                table: "Articulos",
                columns: new[] { "Id", "Descripcion", "Estado", "Existencia", "Marca", "UnidadMedidaId" },
                values: new object[,]
                {
                    { 1, "Papel Bond 8.5x11", 1, 50m, "Report", 2 },
                    { 2, "Tinta para impresora", 1, 20m, "HP", 1 }
                });

            migrationBuilder.InsertData(
                table: "Empleados",
                columns: new[] { "Id", "DepartamentoId", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, 1, 1, "Juan Perez" },
                    { 2, 3, 1, "Maria Gomez" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articulos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Articulos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Proveedores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Proveedores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnidadesMedida",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnidadesMedida",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnidadesMedida",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
