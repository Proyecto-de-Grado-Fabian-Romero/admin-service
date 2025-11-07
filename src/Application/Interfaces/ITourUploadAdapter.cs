using AdminService.Src.Application.DTOs.Create;

namespace AdminService.Src.Application.Interfaces;

public interface ITourUploaderAdapter
{
    Task UploadTourAsync(Guid environmentPublicId, TourUploadDto tourUpload);
}
