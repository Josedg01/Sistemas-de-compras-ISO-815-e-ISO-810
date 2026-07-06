using SistemaDeCompras.DTOs;

namespace SistemaDeCompras.Services;

public interface IOrdenCompraService
{
    Task<List<OrdenCompraDto>> ObtenerTodasAsync(ConsultaOrdenesFiltroDto filtro);
    Task<OrdenCompraDto?> ObtenerPorNumeroAsync(int numero);
    Task<OrdenCompraDto> CrearAsync(OrdenCompraCreateDto dto);
    Task<OrdenCompraDto> AprobarAsync(int numero);
    Task<OrdenCompraDto> RecibirAsync(int numero);
    Task<OrdenCompraDto> CancelarAsync(int numero);
}
