using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<Room> CreateAsync(Room room);
        Task UpdateAsync(int id, Room updated);
        Task DeleteAsync(int id);
    }
}
