using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaDeCompras.Migrations
{
    /// <inheritdoc />
    public partial class PermitirEliminarMaestrosConOrdenesRecibidasOCanceladas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesOrdenCompra_Articulos_ArticuloId",
                table: "DetallesOrdenCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Departamentos_DepartamentoId",
                table: "OrdenesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Empleados_EmpleadoId",
                table: "OrdenesCompra");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "OrdenesCompra",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartamentoId",
                table: "OrdenesCompra",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ArticuloId",
                table: "DetallesOrdenCompra",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesOrdenCompra_Articulos_ArticuloId",
                table: "DetallesOrdenCompra",
                column: "ArticuloId",
                principalTable: "Articulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Departamentos_DepartamentoId",
                table: "OrdenesCompra",
                column: "DepartamentoId",
                principalTable: "Departamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Empleados_EmpleadoId",
                table: "OrdenesCompra",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesOrdenCompra_Articulos_ArticuloId",
                table: "DetallesOrdenCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Departamentos_DepartamentoId",
                table: "OrdenesCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenesCompra_Empleados_EmpleadoId",
                table: "OrdenesCompra");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "OrdenesCompra",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartamentoId",
                table: "OrdenesCompra",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArticuloId",
                table: "DetallesOrdenCompra",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesOrdenCompra_Articulos_ArticuloId",
                table: "DetallesOrdenCompra",
                column: "ArticuloId",
                principalTable: "Articulos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Departamentos_DepartamentoId",
                table: "OrdenesCompra",
                column: "DepartamentoId",
                principalTable: "Departamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenesCompra_Empleados_EmpleadoId",
                table: "OrdenesCompra",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
