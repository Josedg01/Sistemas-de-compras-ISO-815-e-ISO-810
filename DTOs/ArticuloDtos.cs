using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record ArticuloDto(
    int Id,
    string Descripcion,
    string Marca,
    int UnidadMedidaId,
    string UnidadMedidaDescripcion,
    decimal Existencia,
    EstadoRegistro Estado);

public record ArticuloUpsertDto(
    string Descripcion,
    string Marca,
    int UnidadMedidaId,
    decimal Existencia,
    EstadoRegistro Estado);
