using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Domain.Interfaces;

public interface ITour360RequestRepository
{
    Task<(List<Tour360Request> Items, int TotalItems)> GetAllAsync(
        Tour360Status? status,
        int page,
        int limit
    );

    Task<Tour360Request> AddAsync(Tour360Request request);

    Task<Tour360Request?> GetByIdAsync(Guid id);

    Task<Tour360Request?> GetByEnvironmentIdAsync(Guid id);

    Task UpdateAsync(Tour360Request request);

    Task<Tour360Request?> GetLastNonCancelledByEnvironmentAsync(Guid environmentPublicId);

    Task<(List<Tour360Request> Items, int TotalItems)> GetAllByScheduledDayAsync(
        Tour360Status? status,
        long startOfDayTimestamp,
        long endOfDayTimestamp,
        int page,
        int limit
    );
}
