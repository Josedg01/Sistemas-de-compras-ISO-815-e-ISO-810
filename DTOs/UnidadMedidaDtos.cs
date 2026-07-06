using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record UnidadMedidaDto(int Id, string Descripcion, EstadoRegistro Estado);

public record UnidadMedidaUpsertDto(string Descripcion, EstadoRegistro Estado);
