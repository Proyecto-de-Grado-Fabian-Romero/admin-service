using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class AdminDebtService(IAdminDebtRepository debtRepository, IMapper mapper) : IAdminDebtService
{
    private readonly IAdminDebtRepository _debtRepository = debtRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedResultDto<AdminDebtDto>> GetOutstandingDebtsAsync(int page, int limit)
    {
        var (debts, totalCount) = await _debtRepository.GetPaginatedDebtsAsync(page, limit);

        return new PaginatedResultDto<AdminDebtDto>
        {
            Page = page,
            Limit = limit,
            TotalItems = totalCount,
            Items = _mapper.Map<List<AdminDebtDto>>(debts),
        };
    }

    public async Task<AdminDebtDetailDto> GetDebtDetailsAsync(Guid debtId)
    {
        var debt = await _debtRepository.GetDebtByIdAsync(debtId);
        return _mapper.Map<AdminDebtDetailDto>(debt);
    }
}