using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Domain.Interfaces;

public interface ITour360RequestRepository
{
    Task<(List<Tour360Request> Items, int TotalItems)> GetAllAsync(Tour360Status? status, int page, int limit);

    Task<Tour360Request> AddAsync(Tour360Request request);
}
