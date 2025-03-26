namespace Prestaciones.Api.Services;

public class AlmacenamientoArchivosService
{
    private readonly string _rutaBase;
    private readonly ILogger<AlmacenamientoArchivosService> _logger;

    public AlmacenamientoArchivosService(IConfiguration configuration, ILogger<AlmacenamientoArchivosService> logger)
    {
        _rutaBase = configuration["Almacenamiento:RutaArchivos"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Archivos");
        _logger = logger;

        if (!Directory.Exists(_rutaBase))
        {
            Directory.CreateDirectory(_rutaBase);
        }
    }

    public async Task<string> GuardarArchivoAsync(string nombreOriginal, Stream contenido)
    {
        try
        {
            var extension = Path.GetExtension(nombreOriginal);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta = Path.Combine(_rutaBase, nombreArchivo);

            using var stream = File.Create(rutaCompleta);
            await contenido.CopyToAsync(stream);

            _logger.LogInformation("Archivo guardado correctamente: {ruta}", rutaCompleta);
            return nombreArchivo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar el archivo {nombre}", nombreOriginal);
            throw;
        }
    }

    public string ObtenerRutaCompleta(string nombreArchivo)
    {
        return Path.Combine(_rutaBase, nombreArchivo);
    }
} 