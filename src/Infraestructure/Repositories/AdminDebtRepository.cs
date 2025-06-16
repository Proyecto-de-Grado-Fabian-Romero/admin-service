using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class AdminDebtRepository(AppDbContext context) : IAdminDebtRepository
{
    private readonly AppDbContext _context = context;

    public async Task<(List<OwnerDebt>, int)> GetPaginatedDebtsAsync(int page, int limit)
    {
        var query = _context.OwnerDebts
            .Where(d => d.TotalAmount > 0)
            .OrderBy(d => d.UpdatedAt);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, total);
    }

    public async Task<OwnerDebt?> GetDebtByIdAsync(Guid debtId)
    {
        return await _context.OwnerDebts
            .FirstOrDefaultAsync(d => d.Id == debtId);
    }
}