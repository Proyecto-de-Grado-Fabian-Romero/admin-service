using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class OwnerEarningRepository(AppDbContext context) : IOwnerEarningRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(OwnerEarning ownerEarning)
    {
        await _context.OwnerEarnings.AddAsync(ownerEarning);

        var ownerDebt = await _context.OwnerDebts
            .Where(d => d.OwnerId == ownerEarning.OwnerId)
            .FirstOrDefaultAsync();

        if (ownerDebt != null)
        {
            ownerDebt.TotalAmount -= ownerEarning.Amount;

            if (ownerDebt.TotalAmount <= 0)
            {
                ownerDebt.TotalAmount = 0;
            }

            ownerDebt.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _context.OwnerDebts.Update(ownerDebt);
        }
        else
        {
            var newOwnerDebt = new OwnerDebt
            {
                OwnerId = ownerEarning.OwnerId,
                TotalAmount = ownerEarning.Amount,
                Currency = ownerEarning.Currency,
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            };

            await _context.OwnerDebts.AddAsync(newOwnerDebt);
        }

        await _context.SaveChangesAsync();
    }
}
