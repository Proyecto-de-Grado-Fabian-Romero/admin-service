using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Domain.Entities;

namespace AdminService.Src.Application.Interfaces;

public interface IOwnerEarningService
{
    Task<OwnerEarning> CreateOwnerEarningAsync(Guid ownerId, CreateOwnerEarningDto earningDto);
}
