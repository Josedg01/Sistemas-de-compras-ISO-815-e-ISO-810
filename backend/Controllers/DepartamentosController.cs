using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/departamentos")]
public class DepartamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartamentosController(AppDbContext context)
    {
        _context = context;
    }

    private static DepartamentoDto ToDto(Departamento d) => new(d.Id, d.Nombre, d.Estado);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetAll([FromQuery] EstadoRegistro? estado)
    {
        var query = _context.Departamentos.AsNoTracking().AsQueryable();
        if (estado.HasValue)
        {
            query = query.Where(d => d.Estado == estado.Value);
        }

        var result = await query.OrderBy(d => d.Nombre).Select(d => ToDto(d)).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> GetById(int id)
    {
        var departamento = await _context.Departamentos.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (departamento is null) return NotFound();
        return Ok(ToDto(departamento));
    }

    [HttpPost]
    public async Task<ActionResult<DepartamentoDto>> Create(DepartamentoUpsertDto dto)
    {
        var departamento = new Departamento { Nombre = dto.Nombre, Estado = dto.Estado };
        _context.Departamentos.Add(departamento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = departamento.Id }, ToDto(departamento));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> Update(int id, DepartamentoUpsertDto dto)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento is null) return NotFound();

        departamento.Nombre = dto.Nombre;
        departamento.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        return Ok(ToDto(departamento));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento is null) return NotFound();

        var enUso = await _context.Empleados.AnyAsync(e => e.DepartamentoId == id)
            || await _context.OrdenesCompra.AnyAsync(o => o.DepartamentoId == id
                && o.Estado != EstadoOrdenCompra.Recibida && o.Estado != EstadoOrdenCompra.Cancelada);
        if (enUso)
            return BadRequest(new { message = "No se puede eliminar el departamento porque tiene empleados u órdenes de compra pendientes/aprobadas asociadas." });

        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
