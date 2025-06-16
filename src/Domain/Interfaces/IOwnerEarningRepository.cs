using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IOwnerEarningRepository
{
    Task AddAsync(OwnerEarning ownerEarning);
}
