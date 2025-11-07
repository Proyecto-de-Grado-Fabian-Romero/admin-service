using AdminService.src.Application.DTOs.Response;

namespace AdminService.Src.Application.Interfaces;

public interface IEnvironmentServiceAdapter
{
    Task<EnvironmentDetailsResponse?> GetEnvironmentDetailsAsync(Guid environmentPublicId);

    Task UpdateDetectedObjectsAsync(Guid environmentPublicId, Dictionary<string, int> detectedObjects);
}
