using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class OwnerRepository(AppDbContext context) : IOwnerRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<OwnerEarning>> GetEarningsByOwnerAsync(Guid ownerId)
    {
        return await _context.OwnerEarnings
            .Where(e => e.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<List<OwnerPayment>> GetPaymentsByOwnerAsync(Guid ownerId)
    {
        return await _context.OwnerPayments
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<(List<OwnerEarning>, int)> GetPaginatedEarningsAsync(Guid ownerId, int page, int limit)
    {
        var query = _context.OwnerEarnings
            .Where(e => e.OwnerId == ownerId)
            .OrderByDescending(e => e.GeneratedAt);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, total);
    }

    public async Task<(List<OwnerPayment>, int)> GetPaginatedPaymentsAsync(Guid ownerId, int page, int limit)
    {
        var query = _context.OwnerPayments
            .Where(p => p.OwnerId == ownerId)
            .OrderByDescending(p => p.CreatedAt);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, total);
    }

    public async Task<OwnerEarning?> GetEarningByIdAsync(Guid ownerId, Guid earningId)
    {
        return await _context.OwnerEarnings
            .FirstOrDefaultAsync(e => e.OwnerId == ownerId && e.Id == earningId);
    }

    public async Task<OwnerPayment?> GetPaymentByIdAsync(Guid ownerId, Guid paymentId)
    {
        return await _context.OwnerPayments
            .FirstOrDefaultAsync(p => p.OwnerId == ownerId && p.Id == paymentId);
    }
}
