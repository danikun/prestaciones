using System;

namespace Prestaciones.Domain.Models
{
    public class ReclamacionPrevia
    {
        public Guid Id { get; private set; }
        public DateTime FechaEntrada { get; private set; }
        public string RutaDocumento { get; private set; }
        public EstadoReclamacion Estado { get; private set; }
        public DateTime? FechaResolucion { get; private set; }
        public string? MotivoResolucion { get; private set; }

        private ReclamacionPrevia() { }

        public static ReclamacionPrevia Crear(string rutaDocumento)
        {
            return new ReclamacionPrevia
            {
                Id = Guid.NewGuid(),
                FechaEntrada = DateTime.UtcNow,
                RutaDocumento = rutaDocumento,
                Estado = EstadoReclamacion.Pendiente
            };
        }

        public void Resolver(EstadoReclamacion nuevoEstado, string motivo)
        {
            if (Estado != EstadoReclamacion.Pendiente)
                throw new InvalidOperationException("Solo se pueden resolver reclamaciones pendientes");

            if (nuevoEstado == EstadoReclamacion.Pendiente)
                throw new InvalidOperationException("El nuevo estado no puede ser pendiente");

            Estado = nuevoEstado;
            FechaResolucion = DateTime.UtcNow;
            MotivoResolucion = motivo;
        }
    }
} 