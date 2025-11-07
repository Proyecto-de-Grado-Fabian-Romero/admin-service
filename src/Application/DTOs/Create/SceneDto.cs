namespace AdminService.Src.Application.DTOs.Create;

public class SceneDto
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string FileId { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public List<POIDto> Pois { get; set; } = [];
}
