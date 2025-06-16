using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Create;

public class CreateOwnerEarningDto
{
    public Guid OwnerId { get; set; }

    public Guid ReservationId { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; } = Currency.Bs;

    public long GeneratedAt { get; set; }
}
