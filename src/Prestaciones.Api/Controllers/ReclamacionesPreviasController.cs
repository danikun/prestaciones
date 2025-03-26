using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Prestaciones.Application.DTOs;
using Prestaciones.Application.Services;
using Prestaciones.Api.Services;
using Prestaciones.Api.DTOs;

namespace Prestaciones.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReclamacionesPreviasController : ControllerBase
    {
        private readonly ReclamacionPreviaService _service;
        private readonly AlmacenamientoArchivosService _almacenamiento;

        public ReclamacionesPreviasController(ReclamacionPreviaService service, AlmacenamientoArchivosService almacenamiento)
        {
            _service = service;
            _almacenamiento = almacenamiento;
        }

        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10MB
        public async Task<ActionResult<ReclamacionPreviaDto>> Crear([FromForm] SubirReclamacionPreviaDto dto)
        {
            if (dto.Documento == null || dto.Documento.Length == 0)
                return BadRequest("No se ha proporcionado ningún archivo");

            var nombreArchivo = await _almacenamiento.GuardarArchivoAsync(
                dto.Documento.FileName,
                dto.Documento.OpenReadStream()
            );

            var reclamacion = await _service.CrearAsync(new NuevaReclamacionPreviaDto 
            { 
                RutaDocumento = $"/archivos/{nombreArchivo}" 
            });

            return CreatedAtAction(nameof(ObtenerPorId), new { id = reclamacion.Id }, reclamacion);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReclamacionPreviaDto>>> Obtener([FromQuery] string? estado = null)
        {
            var reclamaciones = estado == null 
                ? await _service.ObtenerTodasAsync()
                : await _service.ObtenerPorEstadoAsync(estado);
            return Ok(reclamaciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReclamacionPreviaDto>> ObtenerPorId(Guid id)
        {
            try
            {
                var reclamacion = await _service.ObtenerPorIdAsync(id);
                return Ok(reclamacion);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/resolver")]
        public async Task<ActionResult<ReclamacionPreviaDto>> Resolver(Guid id, [FromBody] ResolverReclamacionPreviaDto dto)
        {
            var reclamacion = await _service.ResolverAsync(id, dto);
            return Ok(reclamacion);
        }

        [HttpGet("{id}/documento")]
        public async Task<IActionResult> DescargarDocumento(Guid id)
        {
            try
            {
                var reclamacion = await _service.ObtenerPorIdAsync(id);
                var nombreArchivo = reclamacion.RutaDocumento.Split('/').Last();
                var rutaCompleta = _almacenamiento.ObtenerRutaCompleta(nombreArchivo);

                if (!System.IO.File.Exists(rutaCompleta))
                    return NotFound("No se encontró el archivo");

                var contentType = Path.GetExtension(rutaCompleta).ToLowerInvariant() switch
                {
                    ".pdf" => "application/pdf",
                    ".doc" => "application/msword",
                    ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    _ => "application/octet-stream"
                };

                return PhysicalFile(rutaCompleta, contentType, nombreArchivo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No se encontró la reclamación");
            }
        }
    }
} 