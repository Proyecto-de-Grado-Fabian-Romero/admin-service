namespace AdminService.Src.Application.DTOs.Response;

public record UploadTourResponse
{
    public bool Success { get; set; }
    public string Error { get; set; } = string.Empty;
}
