using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Interfaces;
using AdminService.Src.Infraestructure.Data;

namespace AdminService.Src.Infraestructure.Repositories;

public class OwnerEarningRepository(AppDbContext context) : IOwnerEarningRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddAsync(OwnerEarning ownerEarning)
    {
        await _context.OwnerEarnings.AddAsync(ownerEarning);
        await _context.SaveChangesAsync();
    }
}
