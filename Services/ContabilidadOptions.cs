namespace SistemaDeCompras.Services;

public class ContabilidadOptions
{
    public const string SectionName = "Contabilidad";

    public string BaseUrl { get; set; } = string.Empty;
    public string AsientosEndpoint { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 15;
}
