using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class OwnerEarningService(
    IOwnerEarningRepository earningRepository,
    IMapper mapper,
    IOwnerDebtRepository debtRepository
) : IOwnerEarningService
{
    private readonly IOwnerEarningRepository _earningRepository = earningRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IOwnerDebtRepository _debtRepository = debtRepository;

    public async Task<OwnerEarning> CreateOwnerEarningAsync(
        Guid ownerId,
        CreateOwnerEarningDto earningDto
    )
    {
        var ownerEarning = _mapper.Map<OwnerEarning>(earningDto);
        ownerEarning.OwnerId = ownerId;

        await _earningRepository.AddAsync(ownerEarning);

        await _debtRepository.GetByOwnerIdAsync(ownerId);

        Console.WriteLine($"Owner Earning Amount: {ownerEarning.Amount}");

        return ownerEarning;
    }

    public async Task<MonthlyEarningsResponseDto> GetMonthlyEarningsAsync(
        Guid ownerId,
        long fromMs,
        long toMs
    )
    {
        var rows = await _earningRepository.GetMonthlyTotalsAsync(ownerId, fromMs, toMs);

        var points = rows.Select(t => new MonthlyEarningPointDto
            {
                Year = t.Year,
                Month = t.Month,
                Total = t.Total,
                Currency = "BOB",
            })
            .ToList();

        return new MonthlyEarningsResponseDto
        {
            OwnerId = ownerId,
            Currency = "BOB",
            Points = points,
        };
    }
}
