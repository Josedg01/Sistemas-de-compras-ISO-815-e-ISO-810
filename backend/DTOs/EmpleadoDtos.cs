using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record EmpleadoDto(int Id, string Nombre, int DepartamentoId, string DepartamentoNombre, EstadoRegistro Estado);

public record EmpleadoUpsertDto(string Nombre, int DepartamentoId, EstadoRegistro Estado);
