using Application.Repository;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories;

public class BookingRepository(BookDbContext context) : IBookingRepository
{
    private readonly BookDbContext _context = context;

    public async Task<IEnumerable<Booking>> GetAllAsync() =>
        await _context.Bookings.AsNoTracking().ToListAsync();

    public async Task<Booking?> GetByIdAsync(int id) =>
        await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Bookings.FindAsync(id);
        if (entity == null) return;
        _context.Bookings.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
