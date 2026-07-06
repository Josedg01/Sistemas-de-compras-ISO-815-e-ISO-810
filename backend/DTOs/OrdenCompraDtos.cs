using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record OrdenCompraDetalleDto(
    int Id,
    int ArticuloId,
    string ArticuloDescripcion,
    decimal Cantidad,
    int UnidadMedidaId,
    string UnidadMedidaDescripcion,
    decimal CostoUnitario,
    decimal Subtotal);

public record OrdenCompraDetalleUpsertDto(
    int ArticuloId,
    decimal Cantidad,
    int UnidadMedidaId,
    decimal CostoUnitario);

public record OrdenCompraDto(
    int Numero,
    DateTime FechaOrden,
    EstadoOrdenCompra Estado,
    int ProveedorId,
    string ProveedorNombre,
    int DepartamentoId,
    string DepartamentoNombre,
    int EmpleadoId,
    string EmpleadoNombre,
    decimal Total,
    List<OrdenCompraDetalleDto> Detalles);

public record OrdenCompraCreateDto(
    DateTime FechaOrden,
    int ProveedorId,
    int DepartamentoId,
    int EmpleadoId,
    List<OrdenCompraDetalleUpsertDto> Detalles);

public record OrdenCompraEstadoUpdateDto(EstadoOrdenCompra Estado);
