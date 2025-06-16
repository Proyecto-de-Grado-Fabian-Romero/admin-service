using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Repositories;

public class AdminPaymentRepository(AppDbContext context) : IAdminPaymentRepository
{
    private readonly AppDbContext _context = context;

    public async Task<(List<OwnerPayment>, int)> GetPaginatedPaymentsAsync(int page, int limit)
    {
        var query = _context.OwnerPayments
            .OrderByDescending(p => p.CreatedAt);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (items, total);
    }

    public async Task<OwnerPayment?> GetPaymentByIdAsync(Guid paymentId)
    {
        return await _context.OwnerPayments
            .FirstOrDefaultAsync(p => p.Id == paymentId);
    }

    public async Task AddAsync(OwnerPayment ownerPayment)
    {
        await _context.OwnerPayments.AddAsync(ownerPayment);
        await _context.SaveChangesAsync();
    }
}
