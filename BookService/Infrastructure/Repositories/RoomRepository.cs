using Application.Repository;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BookDbContext _context;
        public RoomRepository(BookDbContext context) => _context = context;
        public async Task<IEnumerable<Room>> GetAllAsync() =>
            await _context.Rooms.AsNoTracking().ToListAsync();
        public async Task<Room?> GetByIdAsync(int id) =>
            await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        public async Task AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Rooms.FindAsync(id);
            if (entity == null) return;
            _context.Rooms.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
