using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class Tour360RequestRepository(AppDbContext context) : ITour360RequestRepository
{
    private readonly AppDbContext _context = context;

    public async Task<(List<Tour360Request> Items, int TotalItems)> GetAllAsync(Tour360Status? status, int page, int limit)
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
}
