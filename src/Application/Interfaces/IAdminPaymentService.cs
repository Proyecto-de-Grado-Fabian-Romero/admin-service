using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;

namespace AdminService.Src.Application.Interfaces;

public interface IAdminPaymentService
{
    Task<PaginatedResultDto<AdminPaymentDto>> GetPaymentsAsync(int page, int limit);

    Task<AdminPaymentDetailDto> GetPaymentDetailsAsync(Guid paymentId);
}
