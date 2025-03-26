using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prestaciones.Domain.Models;
using Prestaciones.Domain.Repositories;
using Prestaciones.Infrastructure.Data;

namespace Prestaciones.Infrastructure.Repositories
{
    public class ReclamacionPreviaRepository : IReclamacionPreviaRepository
    {
        private readonly PrestacionesDbContext _context;

        public ReclamacionPreviaRepository(PrestacionesDbContext context)
        {
            _context = context;
        }

        public async Task<ReclamacionPrevia> ObtenerPorIdAsync(Guid id)
        {
            var reclamacion = await _context.ReclamacionesPrevias.FindAsync(id);
            if (reclamacion == null)
                throw new KeyNotFoundException($"No se encontró la reclamación con ID {id}");
            return reclamacion;
        }

        public async Task<IEnumerable<ReclamacionPrevia>> ObtenerPorEstadoAsync(EstadoReclamacion estado)
        {
            return await _context.ReclamacionesPrevias
                .Where(r => r.Estado == estado)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReclamacionPrevia>> ObtenerTodasAsync()
        {
            return await _context.ReclamacionesPrevias.ToListAsync();
        }

        public async Task GuardarAsync(ReclamacionPrevia reclamacion)
        {
            await _context.ReclamacionesPrevias.AddAsync(reclamacion);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(ReclamacionPrevia reclamacion)
        {
            _context.ReclamacionesPrevias.Update(reclamacion);
            await _context.SaveChangesAsync();
        }
    }
} 