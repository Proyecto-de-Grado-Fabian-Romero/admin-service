namespace AdminService.Src.Application.Interfaces;

public interface IEnvironmentServiceAdapter
{
    Task<string?> GetEnvironmentNameAsync(Guid environmentPublicId);
}
