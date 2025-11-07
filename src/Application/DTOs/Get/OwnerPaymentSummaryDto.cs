namespace AdminService.Src.Application.DTOs.Get;

public class OwnerPaymentSummaryDto
{
    public decimal TotalEarnings { get; set; }

    public decimal TotalPaid { get; set; }

    public decimal OutstandingDebt => TotalEarnings - TotalPaid;

    public List<ChartBarDto> MonthlyChart { get; set; } = [];
}
