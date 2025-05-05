using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;

namespace AdminService.Src.Infraestructure.Adapters;

public class TourUploaderAdapter(HttpClient httpClient) : ITourUploaderAdapter
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task UploadTourAsync(Guid environmentPublicId, TourUploadDto tourUpload)
    {
        var url = $"http://localhost:5150/api/tours?environmentPublicId={environmentPublicId}";
        var response = await _httpClient.PostAsJsonAsync(url, tourUpload);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error al subir el Tour 360.");
        }
    }
}
