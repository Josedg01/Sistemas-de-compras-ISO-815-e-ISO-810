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

    private bool esUnRNCValido(string pRNC)
    {
        if (string.IsNullOrWhiteSpace(pRNC)) return false;
        string vcRNC = pRNC.Replace("-", "").Replace(" ", "");
        
        if (vcRNC.Length == 9)
        {
            // Solo validar que sean 9 numeros
            return vcRNC.All(char.IsDigit);
        }
        else if (vcRNC.Length == 11)
        {
            // Validacion Cedula (11 digitos) - Algoritmo de Luhn Modulo 10
            int vnTotal = 0;
            int[] digitoMult = new int[10] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };

            for (int vDig = 1; vDig <= 10; vDig++)
            {
                int vCalculo = Int32.Parse(vcRNC.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }

            int verificador = Int32.Parse(vcRNC.Substring(10, 1));
            int residuo = vnTotal % 10;
            
            if (residuo == 0 && verificador == 0) return true;
            if ((10 - residuo) == verificador) return true;
            return false;
        }

        return false;
    }

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
        if (!esUnRNCValido(dto.CedulaRnc))
            return BadRequest(new { message = "El RNC/Cédula proporcionado no es válido." });

        if (await _context.Proveedores.AnyAsync(p => p.CedulaRnc == dto.CedulaRnc))
            return BadRequest(new { message = $"Ya existe un proveedor registrado con el RNC/Cédula {dto.CedulaRnc}." });

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
        if (!esUnRNCValido(dto.CedulaRnc))
            return BadRequest(new { message = "El RNC/Cédula proporcionado no es válido." });

        if (await _context.Proveedores.AnyAsync(p => p.CedulaRnc == dto.CedulaRnc && p.Id != id))
            return BadRequest(new { message = $"Ya existe otro proveedor registrado con el RNC/Cédula {dto.CedulaRnc}." });

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
