using AdminService.Src.Application.DTOs.Response;
using AdminService.Src.Application.Interfaces;

namespace AdminService.Src.Infraestructure.Adapters;

public class ObjectDetectionAdapter(HttpClient client) : IObjectDetectionAdapter
{
    private readonly HttpClient _client = client;

    public async Task<Dictionary<string, int>> DetectFromImagesAsync(List<string> imageUrls)
    {
        var requestUrl = "/detect/";

        var response = await _client.PostAsJsonAsync(requestUrl, imageUrls);
        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadFromJsonAsync<Dictionary<string, DetectedObjectResponse>>();

        return raw?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
               ?? [];
    }
}
