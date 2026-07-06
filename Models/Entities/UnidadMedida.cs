using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Models.Entities;

public class UnidadMedida
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

    public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}
