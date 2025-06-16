using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get.Admin;

public class AdminDebtDetailDto
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public decimal TotalAmount { get; set; }

    public Currency Currency { get; set; }

    public string Reference { get; set; } = string.Empty;

    public long UpdatedAt { get; set; }

    public string OwnerName { get; set; } = string.Empty;
}
