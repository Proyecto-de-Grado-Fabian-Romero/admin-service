using AdminService.Src.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Src.Infraestructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tour360Request> Tour360Requests { get; set; }

    public DbSet<OwnerEarning> OwnerEarnings { get; set; }

    public DbSet<OwnerDebt> OwnerDebts { get; set; }

    public DbSet<OwnerPayment> OwnerPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour360Request>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<OwnerPayment>()
            .Property(o => o.Currency)
            .HasConversion<string>();

        modelBuilder.Entity<OwnerPayment>()
            .Property(o => o.PaymentMethod)
            .HasConversion<string>();

        modelBuilder.Entity<OwnerDebt>()
            .Property(o => o.Currency)
            .HasConversion<string>();

        modelBuilder.Entity<OwnerEarning>()
            .Property(o => o.Currency)
            .HasConversion<string>();
    }
}
