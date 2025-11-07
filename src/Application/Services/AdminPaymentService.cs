using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class AdminPaymentService(IAdminPaymentRepository paymentRepository, IMapper mapper) : IAdminPaymentService
{
    private readonly IAdminPaymentRepository _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginatedResultDto<AdminPaymentDto>> GetPaymentsAsync(int page, int limit)
    {
        var (payments, totalCount) = await _paymentRepository.GetPaginatedPaymentsAsync(page, limit);

        return new PaginatedResultDto<AdminPaymentDto>
        {
            Page = page,
            Limit = limit,
            TotalItems = totalCount,
            Items = _mapper.Map<List<AdminPaymentDto>>(payments),
        };
    }

    public async Task<AdminPaymentDetailDto> GetPaymentDetailsAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
        return _mapper.Map<AdminPaymentDetailDto>(payment);
    }
}
