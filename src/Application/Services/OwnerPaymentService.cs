using AdminService.src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class OwnerPaymentService(IOwnerRepository ownerRepository,
    IMapper mapper
) : IOwnerPaymentService
{
    private readonly IOwnerRepository _ownerRepository = ownerRepository;

    private readonly IMapper _mapper = mapper;

    public async Task<OwnerPaymentSummaryDto> GetOwnerPaymentSummaryAsync(string publicId)
    {
        var ownerId = Guid.Parse(publicId);

        var earnings = await _ownerRepository.GetEarningsByOwnerAsync(ownerId);
        var payments = await _ownerRepository.GetPaymentsByOwnerAsync(ownerId);

        var totalEarnings = earnings.Sum(e => e.Amount);
        var totalPaid = payments.Sum(p => p.AmountPaid);

        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        var chartData = earnings
            .Where(e =>
            {
                var dt = DateTimeOffset.FromUnixTimeMilliseconds(e.GeneratedAt).UtcDateTime;
                return dt >= firstDayOfMonth && dt <= lastDayOfMonth;
            })
            .GroupBy(e =>
                new DateTimeOffset(DateTimeOffset.FromUnixTimeMilliseconds(e.GeneratedAt).UtcDateTime.Date).ToUnixTimeMilliseconds())

            .Select(g => new ChartBarDto
            {
                Day = g.Key,
                Amount = g.Sum(e => e.Amount),
            })
            .OrderBy(x => x.Day)
            .ToList();

        return new OwnerPaymentSummaryDto
        {
            TotalEarnings = totalEarnings,
            TotalPaid = totalPaid,
            MonthlyChart = chartData,
        };
    }

    public async Task<PaginatedResultDto<OwnerIncomeDto>> GetOwnerIncomeListAsync(string publicId, int page, int limit)
    {
        var ownerId = Guid.Parse(publicId);

        var (items, total) = await _ownerRepository.GetPaginatedEarningsAsync(ownerId, page, limit);

        return new PaginatedResultDto<OwnerIncomeDto>
        {
            Page = page,
            Limit = limit,
            TotalItems = total,
            Items = _mapper.Map<List<OwnerIncomeDto>>(items),
        };
    }

    public async Task<PaginatedResultDto<OwnerPaymentReceivedDto>> GetOwnerReceivedPaymentsAsync(string publicId, int page, int limit)
    {
        var ownerId = Guid.Parse(publicId);

        var (items, total) = await _ownerRepository.GetPaginatedPaymentsAsync(ownerId, page, limit);

        return new PaginatedResultDto<OwnerPaymentReceivedDto>
        {
            Page = page,
            Limit = limit,
            TotalItems = total,
            Items = _mapper.Map<List<OwnerPaymentReceivedDto>>(items),
        };
    }
}
