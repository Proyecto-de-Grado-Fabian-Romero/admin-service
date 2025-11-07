using AdminService.Src.Application.DTOs.Create;

namespace AdminService.Src.Domain.Events;

public record GetEnvironmentDetailsMessage(Guid environmentPublicId, string correlationId);

public record UpdateDetectedObjectsMessage(
    Guid environmentPublicId,
    Dictionary<string, int> detectedObjects
);

public record UploadTourMessage(
    Guid environmentPublicId,
    TourUploadDto tourUpload,
    string correlationId
);
