using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IOwnerDebtRepository
{
    Task<OwnerDebt?> GetByOwnerIdAsync(Guid ownerId);

    Task AddAsync(OwnerDebt ownerDebt);

    Task UpdateAsync(OwnerDebt ownerDebt);
}
