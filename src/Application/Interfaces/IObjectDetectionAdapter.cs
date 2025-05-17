namespace AdminService.Src.Application.Interfaces;

public interface IObjectDetectionAdapter
{
    Task<Dictionary<string, int>> DetectFromImagesAsync(List<string> imageUrls);
}
