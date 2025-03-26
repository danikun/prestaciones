using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prestaciones.Domain.Models;

namespace Prestaciones.Domain.Repositories
{
    public interface IReclamacionPreviaRepository
    {
        Task<ReclamacionPrevia> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<ReclamacionPrevia>> ObtenerPorEstadoAsync(EstadoReclamacion estado);
        Task<IEnumerable<ReclamacionPrevia>> ObtenerTodasAsync();
        Task GuardarAsync(ReclamacionPrevia reclamacion);
        Task ActualizarAsync(ReclamacionPrevia reclamacion);
    }
} 