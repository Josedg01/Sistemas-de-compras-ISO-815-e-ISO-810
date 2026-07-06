using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class Proveedor
{
    public int Id { get; set; }
    public string CedulaRnc { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

    public ICollection<OrdenCompra> OrdenesCompra { get; set; } = new List<OrdenCompra>();
}
