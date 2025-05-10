using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.Interfaces;

public interface ITour360RequestService
{
    Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(GetTour360RequestsRequest request);

    Task<Tour360RequestDto> CreateAsync(Tour360RequestCreateDto request, string authenticatedUserId);

    Task<bool> Upload360TourAsync(Guid tourRequestId, TourUploadDto uploadDto);

    Task<bool> UpdateStatusAsync(Guid id, Tour360Status newStatus);
}
