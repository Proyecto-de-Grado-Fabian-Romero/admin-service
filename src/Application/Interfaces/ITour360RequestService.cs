using AdminService.Src.Application.DTOs;

namespace AdminService.Src.Application.Interfaces;

public interface ITour360RequestService
{
    Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(GetTour360RequestsRequest request);
}
