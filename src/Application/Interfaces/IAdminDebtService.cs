using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;

namespace AdminService.Src.Application.Interfaces;

public interface IAdminDebtService
{
    Task<PaginatedResultDto<AdminDebtDto>> GetOutstandingDebtsAsync(int page, int limit);

    Task<AdminDebtDetailDto> GetDebtDetailsAsync(Guid debtId);
}
