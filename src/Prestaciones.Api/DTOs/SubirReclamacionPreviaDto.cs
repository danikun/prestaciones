using Microsoft.AspNetCore.Http;

namespace Prestaciones.Api.DTOs;

public class SubirReclamacionPreviaDto
{
    public IFormFile Documento { get; set; } = null!;
} 