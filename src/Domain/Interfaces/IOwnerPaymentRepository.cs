using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IOwnerRepository
{
    Task<List<OwnerEarning>> GetEarningsByOwnerAsync(Guid ownerId);

    Task<List<OwnerPayment>> GetPaymentsByOwnerAsync(Guid ownerId);

    Task<(List<OwnerEarning> Items, int TotalCount)> GetPaginatedEarningsAsync(Guid ownerId, int page, int limit);

    Task<(List<OwnerPayment> Items, int TotalCount)> GetPaginatedPaymentsAsync(Guid ownerId, int page, int limit);

    Task<OwnerEarning?> GetEarningByIdAsync(Guid ownerId, Guid earningId);

    Task<OwnerPayment?> GetPaymentByIdAsync(Guid ownerId, Guid paymentId);
}
