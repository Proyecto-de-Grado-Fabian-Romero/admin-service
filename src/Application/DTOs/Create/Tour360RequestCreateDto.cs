using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Create;

public class Tour360RequestCreateDto
{
    public Guid EnvironmentId { get; set; }

    public Guid OwnerId { get; set; }
}
