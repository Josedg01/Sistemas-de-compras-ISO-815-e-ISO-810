using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Controllers;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProveedoresController(AppDbContext context)
    {
        _context = context;
    }

    private static ProveedorDto ToDto(Proveedor p) => new(p.Id, p.CedulaRnc, p.NombreComercial, p.Estado);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll([FromQuery] EstadoRegistro? estado)
    {
        var query = _context.Proveedores.AsNoTracking().AsQueryable();
        if (estado.HasValue)
        {
            query = query.Where(p => p.Estado == estado.Value);
        }

        var result = await query.OrderBy(p => p.NombreComercial).Select(p => ToDto(p)).ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProveedorDto>> GetById(int id)
    {
        var proveedor = await _context.Proveedores.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (proveedor is null) return NotFound();
        return Ok(ToDto(proveedor));
    }

    [HttpPost]
    public async Task<ActionResult<ProveedorDto>> Create(ProveedorUpsertDto dto)
    {
        var proveedor = new Proveedor
        {
            CedulaRnc = dto.CedulaRnc,
            NombreComercial = dto.NombreComercial,
            Estado = dto.Estado
        };
        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, ToDto(proveedor));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProveedorUpsertDto dto)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor is null) return NotFound();

        proveedor.CedulaRnc = dto.CedulaRnc;
        proveedor.NombreComercial = dto.NombreComercial;
        proveedor.Estado = dto.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor is null) return NotFound();

        proveedor.Estado = EstadoRegistro.Inactivo;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
