using AdminService.src.Application.DTOs.Response;

namespace AdminService.Src.Application.Interfaces;

public interface IEnvironmentServiceAdapter
{
    Task<EnvironmentDetailsResponse?> GetEnvironmentDetailsAsync(Guid environmentPublicId);
}
