using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IAdminDebtRepository
{
    Task<(List<OwnerDebt>, int)> GetPaginatedDebtsAsync(int page, int limit);

    Task<OwnerDebt?> GetDebtByIdAsync(Guid debtId);
}
