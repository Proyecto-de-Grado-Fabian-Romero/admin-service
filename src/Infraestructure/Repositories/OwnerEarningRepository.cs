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

        var ownerDebt = await _context
            .OwnerDebts.Where(d => d.OwnerId == ownerEarning.OwnerId)
            .FirstOrDefaultAsync();

        Console.WriteLine($"[EARNINGS] Updating debt for OwnerId: {ownerEarning.OwnerId}");

        if (ownerDebt != null)
        {
            if (ownerDebt.TotalAmount <= 0)
            {
                ownerDebt.TotalAmount = 0;
            }

            ownerDebt.TotalAmount += ownerEarning.Amount;
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

    public async Task<IReadOnlyList<(int Year, int Month, decimal Total)>> GetMonthlyTotalsAsync(
        Guid ownerId,
        long fromMs,
        long toMs
    )
    {
        // 1) Trae TODO del owner (para depurar)
        var all = await _context
            .OwnerEarnings.Where(e => e.OwnerId == ownerId)
            .OrderBy(e => e.GeneratedAt)
            .Select(e => new { e.GeneratedAt, e.Amount })
            .ToListAsync();

        Console.WriteLine($"[EARNINGS] total rows for owner: {all.Count}");
        foreach (var e in all)
        {
            var ms = e.GeneratedAt < 100_000_000_000L ? e.GeneratedAt * 1000L : e.GeneratedAt; // por si hay registros en segundos
            var dt = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
        }

        // 2) Filtra por rango en ms (tal cual)
        var rows = all.Where(e => e.GeneratedAt >= fromMs && e.GeneratedAt < toMs).ToList();

        // 3) Agrupa N-O-M-B-R-A-N-D-O el tuple y formatea el total
        var grouped = rows.GroupBy(e =>
            {
                var ms = e.GeneratedAt < 100_000_000_000L ? e.GeneratedAt * 1000L : e.GeneratedAt;
                var dt = DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime;
                return new { dt.Year, dt.Month };
            })
            .Select(g => (Year: g.Key.Year, Month: g.Key.Month, Total: g.Sum(x => x.Amount)))
            .OrderBy(t => t.Year)
            .ThenBy(t => t.Month)
            .ToList();

        return grouped;
    }
}
