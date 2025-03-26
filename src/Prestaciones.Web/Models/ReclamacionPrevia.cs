namespace Prestaciones.Web.Models;

public class ReclamacionPrevia
{
    public Guid Id { get; set; }
    public DateTime FechaEntrada { get; set; }
    public string RutaDocumento { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaResolucion { get; set; }
    public string? MotivoResolucion { get; set; }
} 