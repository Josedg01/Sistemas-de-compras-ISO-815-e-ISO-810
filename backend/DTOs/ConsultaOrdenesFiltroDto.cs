using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public class ConsultaOrdenesFiltroDto
{
    public int? DepartamentoId { get; set; }
    public int? EmpleadoId { get; set; }
    public int? ProveedorId { get; set; }
    public EstadoOrdenCompra? Estado { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
