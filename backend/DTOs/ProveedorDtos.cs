using SistemaDeCompras.Models.Enums;

namespace SistemaDeCompras.DTOs;

public record ProveedorDto(int Id, string CedulaRnc, string NombreComercial, EstadoRegistro Estado);

public record ProveedorUpsertDto(string CedulaRnc, string NombreComercial, EstadoRegistro Estado);
