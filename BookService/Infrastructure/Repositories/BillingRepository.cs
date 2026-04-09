using Application.Repository;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories;

public class BillingRepository(BookDbContext context) : IBillingRepository
{
    private readonly BookDbContext _context = context;

    public async Task<IEnumerable<Billing>> GetAllAsync() =>
    await _context.Billings
        .Include(b => b.Booking)
            .ThenInclude(bk => bk.Customer)
        .Include(b => b.Booking)
            .ThenInclude(bk => bk.Room)
        .AsNoTracking()
        .ToListAsync();

    public async Task<Billing?> GetByIdAsync(int id) =>
    await _context.Billings
        .Include(b => b.Booking)
            .ThenInclude(bk => bk.Customer)
        .Include(b => b.Booking)
            .ThenInclude(bk => bk.Room)
        .FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Billing billing)
    {
        _context.Billings.Add(billing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Billing billing)
    {
        _context.Billings.Update(billing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Billings.FindAsync(id);
        if (entity == null) return;
        _context.Billings.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
