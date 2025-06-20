using AdminService.src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get;

namespace AdminService.Src.Application.Interfaces;

public interface IOwnerPaymentService
{
    Task<OwnerPaymentSummaryDto> GetOwnerPaymentSummaryAsync(string publicId);

    Task<PaginatedResultDto<OwnerIncomeDto>> GetOwnerIncomeListAsync(string publicId, int page, int limit);

    Task<PaginatedResultDto<OwnerPaymentReceivedDto>> GetOwnerReceivedPaymentsAsync(string publicId, int page, int limit);

    Task<OwnerIncomeDetailDto> GetOwnerIncomeDetailsAsync(string publicId, Guid incomeId);

    Task<OwnerPaymentReceivedDetailDto> GetOwnerReceivedPaymentDetailsAsync(string publicId, Guid paymentId);
}
