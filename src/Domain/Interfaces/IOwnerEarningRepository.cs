using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IOwnerEarningRepository
{
    Task AddAsync(OwnerEarning ownerEarning);

    Task<IReadOnlyList<(int Year, int Month, decimal Total)>> GetMonthlyTotalsAsync(
        Guid ownerId,
        long fromUtc,
        long toUtc
    );
}
