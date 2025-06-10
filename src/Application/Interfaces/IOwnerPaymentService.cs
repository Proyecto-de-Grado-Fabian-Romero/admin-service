using AdminService.Src.Application.DTOs.Get;

namespace AdminService.Src.Application.Interfaces;

public interface IOwnerPaymentService
{
    Task<OwnerPaymentSummaryDto> GetOwnerPaymentSummaryAsync(string publicId);
}
