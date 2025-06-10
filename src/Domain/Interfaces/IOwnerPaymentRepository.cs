using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<List<OwnerEarning>> GetEarningsByOwnerAsync(Guid ownerId);

    Task<List<OwnerPayment>> GetPaymentsByOwnerAsync(Guid ownerId);
}
