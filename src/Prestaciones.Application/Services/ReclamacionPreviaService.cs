using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prestaciones.Application.DTOs;
using Prestaciones.Domain.Models;
using Prestaciones.Domain.Repositories;

namespace Prestaciones.Application.Services
{
    public class ReclamacionPreviaService
    {
        private readonly IReclamacionPreviaRepository _repository;

        public ReclamacionPreviaService(IReclamacionPreviaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReclamacionPreviaDto> CrearAsync(NuevaReclamacionPreviaDto dto)
        {
            var reclamacion = ReclamacionPrevia.Crear(dto.RutaDocumento);
            await _repository.GuardarAsync(reclamacion);
            return MapToDto(reclamacion);
        }

        public async Task<ReclamacionPreviaDto> ObtenerPorIdAsync(Guid id)
        {
            var reclamacion = await _repository.ObtenerPorIdAsync(id);
            return MapToDto(reclamacion);
        }

        public async Task<IEnumerable<ReclamacionPreviaDto>> ObtenerTodasAsync()
        {
            var reclamaciones = await _repository.ObtenerTodasAsync();
            var dtos = new List<ReclamacionPreviaDto>();
            
            foreach (var reclamacion in reclamaciones)
            {
                dtos.Add(MapToDto(reclamacion));
            }

            return dtos;
        }

        public async Task<IEnumerable<ReclamacionPreviaDto>> ObtenerPorEstadoAsync(string estado)
        {
            var estadoEnum = Enum.Parse<EstadoReclamacion>(estado, true);
            var reclamaciones = await _repository.ObtenerPorEstadoAsync(estadoEnum);
            var dtos = new List<ReclamacionPreviaDto>();
            
            foreach (var reclamacion in reclamaciones)
            {
                dtos.Add(MapToDto(reclamacion));
            }

            return dtos;
        }

        public async Task<ReclamacionPreviaDto> ResolverAsync(Guid id, ResolverReclamacionPreviaDto dto)
        {
            var reclamacion = await _repository.ObtenerPorIdAsync(id);
            var nuevoEstado = Enum.Parse<EstadoReclamacion>(dto.Estado, true);
            
            reclamacion.Resolver(nuevoEstado, dto.Motivo);
            await _repository.ActualizarAsync(reclamacion);
            
            return MapToDto(reclamacion);
        }

        private static ReclamacionPreviaDto MapToDto(ReclamacionPrevia reclamacion)
        {
            return new ReclamacionPreviaDto
            {
                Id = reclamacion.Id,
                FechaEntrada = reclamacion.FechaEntrada,
                RutaDocumento = reclamacion.RutaDocumento,
                Estado = reclamacion.Estado.ToString(),
                FechaResolucion = reclamacion.FechaResolucion,
                MotivoResolucion = reclamacion.MotivoResolucion
            };
        }
    }
} 