using AdminService.src.Domain.Enums;

namespace AdminService.Src.Domain.Entities;

public class OwnerEarning
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OwnerId { get; set; }

    public Guid ReservationId { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; } = Currency.Bs;

    public long GeneratedAt { get; set; }
}
