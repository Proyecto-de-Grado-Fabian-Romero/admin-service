using AdminService.src.Application.DTOs.Response;
using AdminService.Src.Application.Interfaces;

namespace AdminService.Src.Infraestructure.Adapters;

public class EnvironmentServiceAdapter(HttpClient httpClient) : IEnvironmentServiceAdapter
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<EnvironmentDetailsResponse?> GetEnvironmentDetailsAsync(Guid environmentPublicId)
    {
        var url = $"http://localhost:5150/api/environments/single?publicId={environmentPublicId}";
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
}
