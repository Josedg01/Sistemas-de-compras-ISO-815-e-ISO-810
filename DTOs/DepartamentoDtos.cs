using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record DepartamentoDto(int Id, string Nombre, EstadoRegistro Estado);

public record DepartamentoUpsertDto(string Nombre, EstadoRegistro Estado);
