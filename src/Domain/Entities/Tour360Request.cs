using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Domain.Entities;

public class Tour360Request
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PublicId { get; set; } = Guid.NewGuid();

    public Guid EnvironmentId { get; set; }

    public Guid OwnerId { get; set; }

    public long RequestDate { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public long? ScheduledDate { get; set; }

    required public Tour360Status Status { get; set; } = Tour360Status.Pending;

    public string? TechnicianName { get; set; } = string.Empty;

    public string? Notes { get; set; } = string.Empty;
}