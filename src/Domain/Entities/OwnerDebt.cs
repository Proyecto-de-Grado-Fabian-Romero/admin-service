using AdminService.src.Domain.Enums;

namespace AdminService.Src.Domain.Entities;

public class OwnerDebt
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OwnerId { get; set; }

    public decimal TotalAmount { get; set; }

    public Currency Currency { get; set; } = Currency.Bs;

    public long UpdatedAt { get; set; }
}
