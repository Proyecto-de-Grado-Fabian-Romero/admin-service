using AdminService.Src.Domain.Entities;
using AdminService.src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class Tour360RequestRepository : ITour360RequestRepository
{
    private readonly AppDbContext _context;

    public Tour360RequestRepository(AppDbContext context)
    {
        _context = context;
    }

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
}
