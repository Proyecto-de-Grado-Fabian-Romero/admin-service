using AdminService.Src.Application.Interfaces;

namespace AdminService.Src.Infrastructure.Adapters;

public class EnvironmentServiceAdapter(HttpClient httpClient) : IEnvironmentServiceAdapter
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> GetEnvironmentNameAsync(Guid environmentPublicId)
    {
        var url = $"http://localhost:5150/api/environments/single?publicId={environmentPublicId}";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<EnvironmentResponse>(url);

            return response?.Title;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private class EnvironmentResponse
    {
        public string Title { get; set; } = string.Empty;
    }
}
