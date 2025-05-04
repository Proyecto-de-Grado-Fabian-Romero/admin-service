using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Create;

public class Tour360RequestCreateDto
{
    public Guid EnvironmentId { get; set; }

    public Guid OwnerId { get; set; }

    public long RequestDate { get; set; }

    public long? ScheduledDate { get; set; }

    public Tour360Status Status { get; set; }

    public string? TechnicianName { get; set; }

    public string? Notes { get; set; }
}
