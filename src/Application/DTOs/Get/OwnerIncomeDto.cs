using System;
using AdminService.Src.Domain.Enums;

namespace AdminService.src.Application.DTOs.Get;

public class OwnerIncomeDto
{
    public Guid ReservationId { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; }

    public long GeneratedAt { get; set; }
}
