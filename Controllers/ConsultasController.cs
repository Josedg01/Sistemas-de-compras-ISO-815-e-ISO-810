using Microsoft.AspNetCore.Mvc;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Services;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly IOrdenCompraService _ordenCompraService;

    public ConsultasController(IOrdenCompraService ordenCompraService)
    {
        _ordenCompraService = ordenCompraService;
    }

    [HttpGet("ordenes-compra")]
    public async Task<ActionResult<IEnumerable<OrdenCompraDto>>> ConsultarOrdenesCompra([FromQuery] ConsultaOrdenesFiltroDto filtro)
    {
        return Ok(await _ordenCompraService.ObtenerTodasAsync(filtro));
    }
}
