using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class OrdenCompra
{
    public int Numero { get; set; }
    public DateTime FechaOrden { get; set; }
    public EstadoOrdenCompra Estado { get; set; } = EstadoOrdenCompra.Pendiente;

    public int? ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }

    public int? DepartamentoId { get; set; }
    public Departamento? Departamento { get; set; }

    public int? EmpleadoId { get; set; }
    public Empleado? Empleado { get; set; }

    public ICollection<OrdenCompraDetalle> Detalles { get; set; } = new List<OrdenCompraDetalle>();
    public ICollection<AsientoContable> AsientosContables { get; set; } = new List<AsientoContable>();
}
