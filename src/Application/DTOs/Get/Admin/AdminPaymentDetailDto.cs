using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get.Admin;

public class AdminPaymentDetailDto
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public decimal AmountPaid { get; set; }

    public Currency Currency { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string Reference { get; set; } = string.Empty;

    public long CreatedAt { get; set; }
}
