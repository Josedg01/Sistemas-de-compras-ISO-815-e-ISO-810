using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class Departamento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

    public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    public ICollection<OrdenCompra> OrdenesCompra { get; set; } = new List<OrdenCompra>();
}
