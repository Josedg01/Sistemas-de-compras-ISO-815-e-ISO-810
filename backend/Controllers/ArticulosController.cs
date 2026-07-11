using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/articulos")]
public class ArticulosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ArticulosController(AppDbContext context)
    {
        _context = context;
    }

    private static ArticuloDto ToDto(Articulo a) => new(
        a.Id, a.Descripcion, a.Marca, a.UnidadMedidaId, a.UnidadMedida?.Descripcion ?? string.Empty, a.Existencia, a.Estado);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticuloDto>>> GetAll([FromQuery] EstadoRegistro? estado)
    {
        var query = _context.Articulos.AsNoTracking().Include(a => a.UnidadMedida).AsQueryable();
        if (estado.HasValue)
        {
            query = query.Where(a => a.Estado == estado.Value);
        }

        var result = await query.OrderBy(a => a.Descripcion).Select(a => ToDto(a)).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArticuloDto>> GetById(int id)
    {
        var articulo = await _context.Articulos.AsNoTracking().Include(a => a.UnidadMedida)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (articulo is null) return NotFound();
        return Ok(ToDto(articulo));
    }

    [HttpPost]
    public async Task<ActionResult<ArticuloDto>> Create(ArticuloUpsertDto dto)
    {
        var unidadExiste = await _context.UnidadesMedida.AnyAsync(u => u.Id == dto.UnidadMedidaId);
        if (!unidadExiste) return BadRequest($"La unidad de medida {dto.UnidadMedidaId} no existe.");

        var articulo = new Articulo
        {
            Descripcion = dto.Descripcion,
            Marca = dto.Marca,
            UnidadMedidaId = dto.UnidadMedidaId,
            Existencia = dto.Existencia,
            Estado = dto.Estado
        };
        _context.Articulos.Add(articulo);
        await _context.SaveChangesAsync();
        await _context.Entry(articulo).Reference(a => a.UnidadMedida).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = articulo.Id }, ToDto(articulo));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ArticuloDto>> Update(int id, ArticuloUpsertDto dto)
    {
        var articulo = await _context.Articulos.FindAsync(id);
        if (articulo is null) return NotFound();

        var unidadExiste = await _context.UnidadesMedida.AnyAsync(u => u.Id == dto.UnidadMedidaId);
        if (!unidadExiste) return BadRequest($"La unidad de medida {dto.UnidadMedidaId} no existe.");

        articulo.Descripcion = dto.Descripcion;
        articulo.Marca = dto.Marca;
        articulo.UnidadMedidaId = dto.UnidadMedidaId;
        articulo.Existencia = dto.Existencia;
        articulo.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        await _context.Entry(articulo).Reference(a => a.UnidadMedida).LoadAsync();
        return Ok(ToDto(articulo));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var articulo = await _context.Articulos.FindAsync(id);
        if (articulo is null) return NotFound();

        var enUso = await _context.DetallesOrdenCompra.AnyAsync(d => d.ArticuloId == id
            && d.OrdenCompra!.Estado != EstadoOrdenCompra.Recibida && d.OrdenCompra.Estado != EstadoOrdenCompra.Cancelada);
        if (enUso)
            return BadRequest(new { message = "No se puede eliminar el artículo porque tiene órdenes de compra pendientes/aprobadas asociadas." });

        _context.Articulos.Remove(articulo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
