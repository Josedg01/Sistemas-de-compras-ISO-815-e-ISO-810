namespace SistemaDeCompras.Services;

public record AsientoContableWsPayload(
    int IdentificadorAsiento,
    string Descripcion,
    int IdentificadorTipoInventario,
    string CuentaContable,
    string TipoMovimiento,
    DateTime FechaAsiento,
    decimal MontoAsiento,
    string Estado);
