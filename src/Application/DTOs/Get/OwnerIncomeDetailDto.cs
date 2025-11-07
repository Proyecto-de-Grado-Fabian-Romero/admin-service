using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get;

public class OwnerIncomeDetailDto
{
    public Guid ReservationId { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; }

    public long GeneratedAt { get; set; }

    public string ReservationDetails { get; set; } = string.Empty;
}
