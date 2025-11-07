using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get;

public class OwnerPaymentReceivedDetailDto
{
    public decimal AmountPaid { get; set; }

    public Currency Currency { get; set; }

    public string Reference { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public long CreatedAt { get; set; }

    public string BankDetails { get; set; } = string.Empty;
}
