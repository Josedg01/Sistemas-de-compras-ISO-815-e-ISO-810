using SistemaDeCompras.Models.Entities;

namespace SistemaDeCompras.Services;

public interface IContabilidadClient
{
    Task<(bool Success, string? Error)> EnviarAsientoAsync(AsientoContable asiento, CancellationToken ct = default);
}
