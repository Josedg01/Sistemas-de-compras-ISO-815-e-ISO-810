using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmpleadosController(AppDbContext context)
    {
        _context = context;
    }

    private static EmpleadoDto ToDto(Empleado e) => new(
        e.Id, e.Nombre, e.DepartamentoId, e.Departamento?.Nombre ?? string.Empty, e.Estado);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetAll(
        [FromQuery] EstadoRegistro? estado, [FromQuery] int? departamentoId)
    {
        var query = _context.Empleados.AsNoTracking().Include(e => e.Departamento).AsQueryable();
        if (estado.HasValue)
        {
            query = query.Where(e => e.Estado == estado.Value);
        }
        if (departamentoId.HasValue)
        {
            query = query.Where(e => e.DepartamentoId == departamentoId.Value);
        }

        var result = await query.OrderBy(e => e.Nombre).Select(e => ToDto(e)).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmpleadoDto>> GetById(int id)
    {
        var empleado = await _context.Empleados.AsNoTracking().Include(e => e.Departamento)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (empleado is null) return NotFound();
        return Ok(ToDto(empleado));
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadoDto>> Create(EmpleadoUpsertDto dto)
    {
        var departamentoExiste = await _context.Departamentos.AnyAsync(d => d.Id == dto.DepartamentoId);
        if (!departamentoExiste) return BadRequest($"El departamento {dto.DepartamentoId} no existe.");

        var empleado = new Empleado
        {
            Nombre = dto.Nombre,
            DepartamentoId = dto.DepartamentoId,
            Estado = dto.Estado
        };
        _context.Empleados.Add(empleado);
        await _context.SaveChangesAsync();
        await _context.Entry(empleado).Reference(e => e.Departamento).LoadAsync();
        return CreatedAtAction(nameof(GetById), new { id = empleado.Id }, ToDto(empleado));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmpleadoDto>> Update(int id, EmpleadoUpsertDto dto)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        var departamentoExiste = await _context.Departamentos.AnyAsync(d => d.Id == dto.DepartamentoId);
        if (!departamentoExiste) return BadRequest($"El departamento {dto.DepartamentoId} no existe.");

        empleado.Nombre = dto.Nombre;
        empleado.DepartamentoId = dto.DepartamentoId;
        empleado.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        await _context.Entry(empleado).Reference(e => e.Departamento).LoadAsync();
        return Ok(ToDto(empleado));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();

        var enUso = await _context.OrdenesCompra.AnyAsync(o => o.EmpleadoId == id
            && o.Estado != EstadoOrdenCompra.Recibida && o.Estado != EstadoOrdenCompra.Cancelada);
        if (enUso)
            return BadRequest(new { message = "No se puede eliminar el empleado porque tiene órdenes de compra pendientes/aprobadas asociadas." });

        _context.Empleados.Remove(empleado);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
