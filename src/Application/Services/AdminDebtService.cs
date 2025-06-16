using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class AdminDebtService(IAdminDebtRepository debtRepository, IAdminPaymentRepository adminPaymentRepository, IOwnerDebtRepository ownerDebtRepository, IMapper mapper) : IAdminDebtService
{
    private readonly IAdminDebtRepository _debtRepository = debtRepository;
    private readonly IOwnerDebtRepository _ownerDebtRepository = ownerDebtRepository;
    private readonly IAdminPaymentRepository _adminPaymentRepository = adminPaymentRepository;
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

    public async Task<AdminPaymentDetailDto> MarkDebtAsPaidAsync(Guid debtId, string reference)
    {
        var ownerDebt = await _debtRepository.GetDebtByIdAsync(debtId) ?? throw new Exception("Debt not found.");
        var ownerPayment = _mapper.Map<OwnerPayment>(ownerDebt);

        ownerPayment.Reference = reference;
        ownerPayment.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        await _adminPaymentRepository.AddAsync(ownerPayment);

        ownerDebt.TotalAmount = 0;
        ownerDebt.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await _ownerDebtRepository.UpdateAsync(ownerDebt);

        return _mapper.Map<AdminPaymentDetailDto>(ownerPayment);
    }
}