using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDeCompras.Models.Entities;

public class OrdenCompraDetalle
{
    public int Id { get; set; }

    public int OrdenCompraNumero { get; set; }
    public OrdenCompra? OrdenCompra { get; set; }

    public int ArticuloId { get; set; }
    public Articulo? Articulo { get; set; }

    public decimal Cantidad { get; set; }

    public int UnidadMedidaId { get; set; }
    public UnidadMedida? UnidadMedida { get; set; }

    public decimal CostoUnitario { get; set; }

    [NotMapped]
    public decimal Subtotal => Cantidad * CostoUnitario;
}
