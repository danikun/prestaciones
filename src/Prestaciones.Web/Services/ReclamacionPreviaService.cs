using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Prestaciones.Web.Models;

namespace Prestaciones.Web.Services;

public class ReclamacionPreviaService
{
    private readonly HttpClient _httpClient;

    public ReclamacionPreviaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ReclamacionPrevia>> GetReclamaciones(string? estado = null)
    {
        var url = estado == null ? "api/reclamacionesprevias" : $"api/reclamacionesprevias?estado={estado}";
        return await _httpClient.GetFromJsonAsync<IEnumerable<ReclamacionPrevia>>(url) ?? Enumerable.Empty<ReclamacionPrevia>();
    }

    public async Task<ReclamacionPrevia?> GetReclamacion(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ReclamacionPrevia>($"api/reclamacionesprevias/{id}");
    }

    public async Task<Stream> DescargarDocumento(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/reclamacionesprevias/{id}/documento");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }

    public async Task<ReclamacionPrevia> CreateReclamacion(IBrowserFile archivo)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024));
        content.Add(fileContent, "documento", archivo.Name);

        var response = await _httpClient.PostAsync("api/reclamacionesprevias", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReclamacionPrevia>() ?? throw new Exception("No se pudo crear la reclamación");
    }

    public async Task<ReclamacionPrevia> ResolverReclamacion(Guid id, string estado, string motivo)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/reclamacionesprevias/{id}/resolver", new { estado, motivo });
        return await response.Content.ReadFromJsonAsync<ReclamacionPrevia>() ?? throw new Exception("No se pudo resolver la reclamación");
    }
} 