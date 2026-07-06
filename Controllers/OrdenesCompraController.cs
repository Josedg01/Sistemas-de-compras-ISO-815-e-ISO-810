using Microsoft.AspNetCore.Mvc;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Services;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/ordenes-compra")]
public class OrdenesCompraController : ControllerBase
{
    private readonly IOrdenCompraService _service;

    public OrdenesCompraController(IOrdenCompraService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenCompraDto>>> GetAll([FromQuery] ConsultaOrdenesFiltroDto filtro)
    {
        return Ok(await _service.ObtenerTodasAsync(filtro));
    }

    [HttpGet("{numero:int}")]
    public async Task<ActionResult<OrdenCompraDto>> GetByNumero(int numero)
    {
        var orden = await _service.ObtenerPorNumeroAsync(numero);
        return orden is null ? NotFound() : Ok(orden);
    }

    [HttpPost]
    public async Task<ActionResult<OrdenCompraDto>> Create(OrdenCompraCreateDto dto)
    {
        try
        {
            var orden = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetByNumero), new { numero = orden.Numero }, orden);
        }
        catch (ReglaNegocioException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{numero:int}/aprobar")]
    public async Task<ActionResult<OrdenCompraDto>> Aprobar(int numero)
    {
        try
        {
            return Ok(await _service.AprobarAsync(numero));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{numero:int}/recibir")]
    public async Task<ActionResult<OrdenCompraDto>> Recibir(int numero)
    {
        try
        {
            return Ok(await _service.RecibirAsync(numero));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{numero:int}/cancelar")]
    public async Task<ActionResult<OrdenCompraDto>> Cancelar(int numero)
    {
        try
        {
            return Ok(await _service.CancelarAsync(numero));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
