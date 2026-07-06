using Microsoft.EntityFrameworkCore;
using SistemaDeCompras.Data;
using SistemaDeCompras.DTOs;
using SistemaDeCompras.Models.Entities;
using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.Services;

public class OrdenCompraService : IOrdenCompraService
{
    private const string CuentaInventario = "1105";
    private const string CuentaCuentasPorPagar = "2101";

    private readonly AppDbContext _context;
    private readonly IContabilidadClient _contabilidadClient;
    private readonly ILogger<OrdenCompraService> _logger;

    public OrdenCompraService(AppDbContext context, IContabilidadClient contabilidadClient, ILogger<OrdenCompraService> logger)
    {
        _context = context;
        _contabilidadClient = contabilidadClient;
        _logger = logger;
    }

    private static OrdenCompraDto ToDto(OrdenCompra o) => new(
        o.Numero,
        o.FechaOrden,
        o.Estado,
        o.ProveedorId,
        o.Proveedor?.NombreComercial ?? string.Empty,
        o.DepartamentoId,
        o.Departamento?.Nombre ?? string.Empty,
        o.EmpleadoId,
        o.Empleado?.Nombre ?? string.Empty,
        o.Detalles.Sum(d => d.Cantidad * d.CostoUnitario),
        o.Detalles.Select(d => new OrdenCompraDetalleDto(
            d.Id,
            d.ArticuloId,
            d.Articulo?.Descripcion ?? string.Empty,
            d.Cantidad,
            d.UnidadMedidaId,
            d.UnidadMedida?.Descripcion ?? string.Empty,
            d.CostoUnitario,
            d.Subtotal)).ToList());

    private IQueryable<OrdenCompra> QueryConIncludes() => _context.OrdenesCompra
        .Include(o => o.Proveedor)
        .Include(o => o.Departamento)
        .Include(o => o.Empleado)
        .Include(o => o.Detalles).ThenInclude(d => d.Articulo)
        .Include(o => o.Detalles).ThenInclude(d => d.UnidadMedida);

    public async Task<List<OrdenCompraDto>> ObtenerTodasAsync(ConsultaOrdenesFiltroDto filtro)
    {
        var query = QueryConIncludes().AsNoTracking().AsQueryable();

        if (filtro.DepartamentoId.HasValue)
            query = query.Where(o => o.DepartamentoId == filtro.DepartamentoId.Value);
        if (filtro.EmpleadoId.HasValue)
            query = query.Where(o => o.EmpleadoId == filtro.EmpleadoId.Value);
        if (filtro.ProveedorId.HasValue)
            query = query.Where(o => o.ProveedorId == filtro.ProveedorId.Value);
        if (filtro.Estado.HasValue)
            query = query.Where(o => o.Estado == filtro.Estado.Value);
        if (filtro.FechaDesde.HasValue)
            query = query.Where(o => o.FechaOrden >= filtro.FechaDesde.Value);
        if (filtro.FechaHasta.HasValue)
            query = query.Where(o => o.FechaOrden <= filtro.FechaHasta.Value);

        var ordenes = await query.OrderByDescending(o => o.Numero).ToListAsync();
        return ordenes.Select(ToDto).ToList();
    }

    public async Task<OrdenCompraDto?> ObtenerPorNumeroAsync(int numero)
    {
        var orden = await QueryConIncludes().AsNoTracking().FirstOrDefaultAsync(o => o.Numero == numero);
        return orden is null ? null : ToDto(orden);
    }

    public async Task<OrdenCompraDto> CrearAsync(OrdenCompraCreateDto dto)
    {
        if (dto.Detalles is null || dto.Detalles.Count == 0)
            throw new ReglaNegocioException("La orden debe tener al menos un detalle.");

        if (!await _context.Proveedores.AnyAsync(p => p.Id == dto.ProveedorId))
            throw new ReglaNegocioException($"El proveedor {dto.ProveedorId} no existe.");
        if (!await _context.Departamentos.AnyAsync(d => d.Id == dto.DepartamentoId))
            throw new ReglaNegocioException($"El departamento {dto.DepartamentoId} no existe.");
        if (!await _context.Empleados.AnyAsync(e => e.Id == dto.EmpleadoId))
            throw new ReglaNegocioException($"El empleado {dto.EmpleadoId} no existe.");

        foreach (var linea in dto.Detalles)
        {
            if (linea.Cantidad <= 0)
                throw new ReglaNegocioException("La cantidad de cada linea debe ser mayor que cero.");
            if (linea.CostoUnitario < 0)
                throw new ReglaNegocioException("El costo unitario no puede ser negativo.");
            if (!await _context.Articulos.AnyAsync(a => a.Id == linea.ArticuloId))
                throw new ReglaNegocioException($"El articulo {linea.ArticuloId} no existe.");
            if (!await _context.UnidadesMedida.AnyAsync(u => u.Id == linea.UnidadMedidaId))
                throw new ReglaNegocioException($"La unidad de medida {linea.UnidadMedidaId} no existe.");
        }

        var orden = new OrdenCompra
        {
            FechaOrden = dto.FechaOrden,
            Estado = EstadoOrdenCompra.Pendiente,
            ProveedorId = dto.ProveedorId,
            DepartamentoId = dto.DepartamentoId,
            EmpleadoId = dto.EmpleadoId,
            Detalles = dto.Detalles.Select(l => new OrdenCompraDetalle
            {
                ArticuloId = l.ArticuloId,
                Cantidad = l.Cantidad,
                UnidadMedidaId = l.UnidadMedidaId,
                CostoUnitario = l.CostoUnitario
            }).ToList()
        };

        _context.OrdenesCompra.Add(orden);
        await _context.SaveChangesAsync();

        var creada = await QueryConIncludes().FirstAsync(o => o.Numero == orden.Numero);
        return ToDto(creada);
    }

    public async Task<OrdenCompraDto> AprobarAsync(int numero)
    {
        var orden = await _context.OrdenesCompra.FirstOrDefaultAsync(o => o.Numero == numero)
            ?? throw new KeyNotFoundException($"No existe la orden {numero}.");

        if (orden.Estado != EstadoOrdenCompra.Pendiente)
            throw new ReglaNegocioException($"Solo se pueden aprobar ordenes en estado Pendiente (estado actual: {orden.Estado}).");

        orden.Estado = EstadoOrdenCompra.Aprobada;
        await _context.SaveChangesAsync();

        return (await ObtenerPorNumeroAsync(numero))!;
    }

    public async Task<OrdenCompraDto> CancelarAsync(int numero)
    {
        var orden = await _context.OrdenesCompra.FirstOrDefaultAsync(o => o.Numero == numero)
            ?? throw new KeyNotFoundException($"No existe la orden {numero}.");

        if (orden.Estado == EstadoOrdenCompra.Recibida)
            throw new ReglaNegocioException("No se puede cancelar una orden que ya fue recibida.");
        if (orden.Estado == EstadoOrdenCompra.Cancelada)
            throw new ReglaNegocioException("La orden ya esta cancelada.");

        orden.Estado = EstadoOrdenCompra.Cancelada;
        await _context.SaveChangesAsync();

        return (await ObtenerPorNumeroAsync(numero))!;
    }

    public async Task<OrdenCompraDto> RecibirAsync(int numero)
    {
        var orden = await _context.OrdenesCompra
            .Include(o => o.Detalles).ThenInclude(d => d.Articulo)
            .FirstOrDefaultAsync(o => o.Numero == numero)
            ?? throw new KeyNotFoundException($"No existe la orden {numero}.");

        if (orden.Estado != EstadoOrdenCompra.Aprobada)
            throw new ReglaNegocioException($"Solo se pueden recibir ordenes en estado Aprobada (estado actual: {orden.Estado}).");

        orden.Estado = EstadoOrdenCompra.Recibida;

        var asientos = new List<AsientoContable>();
        var fecha = DateTime.UtcNow;

        foreach (var linea in orden.Detalles)
        {
            linea.Articulo!.Existencia += linea.Cantidad;

            asientos.Add(new AsientoContable
            {
                Descripcion = $"Recepcion OC {orden.Numero} - {linea.Articulo.Descripcion}",
                TipoInventarioId = linea.ArticuloId,
                CuentaContable = CuentaInventario,
                TipoMovimiento = TipoMovimientoContable.Debito,
                FechaAsiento = fecha,
                MontoAsiento = linea.Subtotal,
                Estado = EstadoAsientoContable.Pendiente,
                OrdenCompraNumero = orden.Numero
            });
        }

        var total = orden.Detalles.Sum(d => d.Subtotal);
        asientos.Add(new AsientoContable
        {
            Descripcion = $"Cuentas por pagar OC {orden.Numero}",
            TipoInventarioId = 0,
            CuentaContable = CuentaCuentasPorPagar,
            TipoMovimiento = TipoMovimientoContable.Credito,
            FechaAsiento = fecha,
            MontoAsiento = total,
            Estado = EstadoAsientoContable.Pendiente,
            OrdenCompraNumero = orden.Numero
        });

        _context.AsientosContables.AddRange(asientos);
        await _context.SaveChangesAsync();

        foreach (var asiento in asientos)
        {
            var (success, error) = await _contabilidadClient.EnviarAsientoAsync(asiento);
            asiento.Estado = success ? EstadoAsientoContable.Enviado : EstadoAsientoContable.Error;
            asiento.FechaEnvio = DateTime.UtcNow;
            asiento.MensajeError = error;
        }

        await _context.SaveChangesAsync();

        return (await ObtenerPorNumeroAsync(numero))!;
    }
}
