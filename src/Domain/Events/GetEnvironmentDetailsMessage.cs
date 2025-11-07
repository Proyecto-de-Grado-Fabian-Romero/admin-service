using AdminService.Src.Application.DTOs.Create;

namespace AdminService.Src.Domain.Events;

public record GetEnvironmentDetailsMessage(Guid EnvironmentPublicId, string CorrelationId);

public record UpdateDetectedObjectsMessage(
    Guid EnvironmentPublicId,
    Dictionary<string, int> DetectedObjects
);

public record UploadTourMessage(
    Guid EnvironmentPublicId,
    TourUploadDto TourUpload,
    string CorrelationId
);
