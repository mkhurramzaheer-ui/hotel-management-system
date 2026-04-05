using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking> CreateAsync(Booking booking);
        Task UpdateAsync(int id, Booking updated);
        Task DeleteAsync(int id);
    }
}
