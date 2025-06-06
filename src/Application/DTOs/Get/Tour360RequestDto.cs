using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get;

public class Tour360RequestDto
{
    public Guid PublicId { get; set; }

    public Guid EnvironmentId { get; set; }

    public string EnvironmentName { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }

    public long RequestDate { get; set; }

    public long? ScheduledDate { get; set; }

    public Tour360Status Status { get; set; }

    public string? TechnicianName { get; set; }

    public string? Notes { get; set; }
}
