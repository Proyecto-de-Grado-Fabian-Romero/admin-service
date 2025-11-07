using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class Tour360RequestRepository(AppDbContext context) : ITour360RequestRepository
{
    private readonly AppDbContext _context = context;

    public async Task<(List<Tour360Request> Items, int TotalItems)> GetAllAsync(
        Tour360Status? status,
        int page,
        int limit
    )
    {
        var query = _context.Tour360Requests.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.RequestDate)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<Tour360Request> AddAsync(Tour360Request request)
    {
        _context.Tour360Requests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<Tour360Request?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Tour360Request>().FirstOrDefaultAsync(t => t.PublicId == id);
    }

    public async Task<Tour360Request?> GetByEnvironmentIdAsync(Guid environmentId)
    {
        return await _context
            .Set<Tour360Request>()
            .Where(t => t.EnvironmentId == environmentId)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Tour360Request request)
    {
        _context.Set<Tour360Request>().Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task<Tour360Request?> GetLastNonCancelledByEnvironmentAsync(
        Guid environmentPublicId
    )
    {
        return await _context
            .Tour360Requests.Where(r =>
                r.EnvironmentId == environmentPublicId && r.Status != Tour360Status.Cancelled
            )
            .OrderByDescending(r => r.RequestDate)
            .FirstOrDefaultAsync();
    }

    public async Task<(List<Tour360Request> Items, int TotalItems)> GetAllByScheduledDayAsync(
        Tour360Status? status,
        long startOfDayTimestamp,
        long endOfDayTimestamp,
        int page,
        int limit
    )
    {
        var query = _context.Tour360Requests.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(r => r.Status == status.Value);
        }

        query = query.Where(r =>
            r.ScheduledDate.HasValue
            && r.ScheduledDate.Value >= startOfDayTimestamp
            && r.ScheduledDate.Value < endOfDayTimestamp
        );

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.RequestDate)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, totalItems);
    }
}
