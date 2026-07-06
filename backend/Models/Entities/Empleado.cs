using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int DepartamentoId { get; set; }
    public Departamento? Departamento { get; set; }
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

    public ICollection<OrdenCompra> OrdenesCompra { get; set; } = new List<OrdenCompra>();
}
