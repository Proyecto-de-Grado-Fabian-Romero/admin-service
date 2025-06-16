using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class OwnerDebtRepository(AppDbContext context) : IOwnerDebtRepository
{
    private readonly AppDbContext _context = context;

    public async Task<OwnerDebt?> GetByOwnerIdAsync(Guid ownerId)
    {
        return await _context.OwnerDebts
            .FirstOrDefaultAsync(d => d.OwnerId == ownerId);
    }

    public async Task AddAsync(OwnerDebt ownerDebt)
    {
        await _context.OwnerDebts.AddAsync(ownerDebt);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(OwnerDebt ownerDebt)
    {
        _context.OwnerDebts.Update(ownerDebt);
        await _context.SaveChangesAsync();
    }
}
