using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Application.Interfaces;

public interface IOwnerEarningService
{
    Task<OwnerEarning> CreateOwnerEarningAsync(Guid ownerId, CreateOwnerEarningDto earningDto);

    Task<MonthlyEarningsResponseDto> GetMonthlyEarningsAsync(
        Guid ownerId,
        long fromUtc,
        long toUtc
    );
}
