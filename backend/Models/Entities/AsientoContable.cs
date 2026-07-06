using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class AsientoContable
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int TipoInventarioId { get; set; }
    public string CuentaContable { get; set; } = string.Empty;
    public TipoMovimientoContable TipoMovimiento { get; set; }
    public DateTime FechaAsiento { get; set; }
    public decimal MontoAsiento { get; set; }
    public EstadoAsientoContable Estado { get; set; } = EstadoAsientoContable.Pendiente;

    public int? OrdenCompraNumero { get; set; }
    public OrdenCompra? OrdenCompra { get; set; }

    public DateTime? FechaEnvio { get; set; }
    public string? MensajeError { get; set; }
}
