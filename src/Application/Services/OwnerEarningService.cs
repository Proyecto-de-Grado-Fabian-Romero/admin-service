using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;

namespace AdminService.Src.Application.Services;

public class OwnerEarningService(IOwnerEarningRepository earningRepository) : IOwnerEarningService
{
    private readonly IOwnerEarningRepository _earningRepository = earningRepository;

    public async Task<OwnerEarning> CreateOwnerEarningAsync(Guid ownerId, CreateOwnerEarningDto earningDto)
    {
        var ownerEarning = new OwnerEarning
        {
            OwnerId = ownerId,
            ReservationId = earningDto.ReservationId,
            Amount = earningDto.Amount,
            Currency = earningDto.Currency,
            GeneratedAt = earningDto.GeneratedAt,
        };

        await _earningRepository.AddAsync(ownerEarning);
        return ownerEarning;
    }
}
