using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class Articulo
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public int UnidadMedidaId { get; set; }
    public UnidadMedida? UnidadMedida { get; set; }
    public decimal Existencia { get; set; }
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

    public ICollection<OrdenCompraDetalle> DetallesOrdenCompra { get; set; } = new List<OrdenCompraDetalle>();
}
