using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Domain.Entities;

public class OwnerPayment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OwnerId { get; set; }

    public decimal AmountPaid { get; set; }

    public Currency Currency { get; set; } = Currency.Bs;

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

    public string Reference { get; set; } = string.Empty;

    public long CreatedAt { get; set; }
}
