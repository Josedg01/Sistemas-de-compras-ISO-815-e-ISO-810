using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/unidades-medida")]
public class UnidadesMedidaController : ControllerBase
{
    private readonly AppDbContext _context;

    public UnidadesMedidaController(AppDbContext context)
    {
        _context = context;
    }

    private static UnidadMedidaDto ToDto(UnidadMedida u) => new(u.Id, u.Descripcion, u.Estado);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadMedidaDto>>> GetAll([FromQuery] EstadoRegistro? estado)
    {
        var query = _context.UnidadesMedida.AsNoTracking().AsQueryable();
        if (estado.HasValue)
        {
            query = query.Where(u => u.Estado == estado.Value);
        }

        var result = await query.OrderBy(u => u.Descripcion).Select(u => ToDto(u)).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnidadMedidaDto>> GetById(int id)
    {
        var unidad = await _context.UnidadesMedida.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (unidad is null) return NotFound();
        return Ok(ToDto(unidad));
    }

    [HttpPost]
    public async Task<ActionResult<UnidadMedidaDto>> Create(UnidadMedidaUpsertDto dto)
    {
        var unidad = new UnidadMedida { Descripcion = dto.Descripcion, Estado = dto.Estado };
        _context.UnidadesMedida.Add(unidad);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = unidad.Id }, ToDto(unidad));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UnidadMedidaDto>> Update(int id, UnidadMedidaUpsertDto dto)
    {
        var unidad = await _context.UnidadesMedida.FindAsync(id);
        if (unidad is null) return NotFound();

        unidad.Descripcion = dto.Descripcion;
        unidad.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        return Ok(ToDto(unidad));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var unidad = await _context.UnidadesMedida.FindAsync(id);
        if (unidad is null) return NotFound();

        var enUso = await _context.Articulos.AnyAsync(a => a.UnidadMedidaId == id)
            || await _context.DetallesOrdenCompra.AnyAsync(d => d.UnidadMedidaId == id);
        if (enUso)
            return BadRequest(new { message = "No se puede eliminar la unidad de medida porque tiene artículos u órdenes de compra asociadas." });

        _context.UnidadesMedida.Remove(unidad);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
