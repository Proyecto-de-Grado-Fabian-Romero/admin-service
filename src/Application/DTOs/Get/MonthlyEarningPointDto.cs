namespace AdminService.Src.Application.DTOs.Get;

public sealed class MonthlyEarningPointDto
{
    public int Year { get; init; }

    public int Month { get; init; } // 1..12

    public decimal Total { get; init; }

    public string Currency { get; init; } = "BOB";
}

public sealed class MonthlyEarningsResponseDto
{
    public Guid OwnerId { get; init; }

    public string Currency { get; init; } = "BOB";

    public List<MonthlyEarningPointDto> Points { get; init; } = [];
}
