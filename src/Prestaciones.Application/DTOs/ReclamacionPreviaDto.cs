using System;

namespace Prestaciones.Application.DTOs
{
    public class ReclamacionPreviaDto
    {
        public Guid Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string RutaDocumento { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string? MotivoResolucion { get; set; }
    }

    public class NuevaReclamacionPreviaDto
    {
        public string RutaDocumento { get; set; } = null!;
    }

    public class ResolverReclamacionPreviaDto
    {
        public string Estado { get; set; }
        public string Motivo { get; set; }
    }
} 