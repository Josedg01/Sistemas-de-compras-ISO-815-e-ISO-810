using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record AsientoContableDto(
    int Id,
    string Descripcion,
    int TipoInventarioId,
    string CuentaContable,
    TipoMovimientoContable TipoMovimiento,
    DateTime FechaAsiento,
    decimal MontoAsiento,
    EstadoAsientoContable Estado,
    int? OrdenCompraNumero,
    DateTime? FechaEnvio,
    string? MensajeError);
