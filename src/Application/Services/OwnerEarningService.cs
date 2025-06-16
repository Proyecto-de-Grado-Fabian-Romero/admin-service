using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class OwnerEarningService(IOwnerEarningRepository earningRepository, IMapper mapper, IOwnerDebtRepository debtRepository) : IOwnerEarningService
{
    private readonly IOwnerEarningRepository _earningRepository = earningRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IOwnerDebtRepository _debtRepository = debtRepository;

    public async Task<OwnerEarning> CreateOwnerEarningAsync(Guid ownerId, CreateOwnerEarningDto earningDto)
    {
        var ownerEarning = _mapper.Map<OwnerEarning>(earningDto);
        ownerEarning.OwnerId = ownerId;

        await _earningRepository.AddAsync(ownerEarning);

        var ownerDebt = await _debtRepository.GetByOwnerIdAsync(ownerId);

        if (ownerDebt != null)
        {
            ownerDebt.TotalAmount -= ownerEarning.Amount;

            if (ownerDebt.TotalAmount <= 0)
            {
                ownerDebt.TotalAmount = 0;
            }

            ownerDebt.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _debtRepository.UpdateAsync(ownerDebt);
        }
        else
        {
            var newDebt = _mapper.Map<OwnerDebt>(ownerEarning);
            await _debtRepository.AddAsync(newDebt);
        }

        return ownerEarning;
    }
}
