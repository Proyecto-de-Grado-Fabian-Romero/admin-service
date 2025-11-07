using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Domain.Interfaces;

public interface IAdminPaymentRepository
{
    Task<(List<OwnerPayment>, int)> GetPaginatedPaymentsAsync(int page, int limit);

    Task<OwnerPayment?> GetPaymentByIdAsync(Guid paymentId);

    Task AddAsync(OwnerPayment ownerPayment);
}
