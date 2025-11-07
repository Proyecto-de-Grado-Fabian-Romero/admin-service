using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get;

public class OwnerPaymentReceivedDto
{
    public decimal AmountPaid { get; set; }

    public Currency Currency { get; set; }

    public string Reference { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public long CreatedAt { get; set; }
}
