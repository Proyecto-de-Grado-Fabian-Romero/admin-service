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
}
