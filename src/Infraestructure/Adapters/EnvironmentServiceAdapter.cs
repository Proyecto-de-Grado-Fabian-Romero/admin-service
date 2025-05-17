using AdminService.src.Application.DTOs.Response;
using AdminService.Src.Application.Interfaces;

namespace AdminService.Src.Infraestructure.Adapters;

public class EnvironmentServiceAdapter(HttpClient httpClient) : IEnvironmentServiceAdapter
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<EnvironmentDetailsResponse?> GetEnvironmentDetailsAsync(Guid environmentPublicId)
    {
        var url = $"/api/environments/single?publicId={environmentPublicId}";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<EnvironmentDetailsResponse>(url);
            return response;
        }
        catch
        {
            return null;
        }
    }

    public async Task UpdateDetectedObjectsAsync(Guid environmentPublicId, Dictionary<string, int> detectedObjects)
    {
        var url = $"/api/environments/{environmentPublicId}/detected-objects";
        var body = new { detectedObjects };

        var response = await _httpClient.PatchAsJsonAsync(url, body);
        response.EnsureSuccessStatusCode();
    }
}
