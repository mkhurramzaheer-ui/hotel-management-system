using Application.Interfaces;
using Application.Repository;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingService(IBookingRepository repository) : IBookingService
    {
        private readonly IBookingRepository _repository = repository;

        public async Task<IEnumerable<Booking>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Booking?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Booking> CreateAsync(Booking booking)
        {
            // 🔥 Check overlapping booking for same room
            var existingBookings = await _repository.GetAllAsync();

            bool isConflict = existingBookings.Any(b =>
                b.RoomId == booking.RoomId &&
                booking.CheckInDate < b.CheckOutDate &&
                booking.CheckOutDate > b.CheckInDate
            );

            if (isConflict)
                throw new Exception("This room is already booked for selected dates.");

            await _repository.AddAsync(booking);

            return booking;
        }
        public async Task UpdateAsync(int id, Booking updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Booking not found.");
            existing.CustomerId = updated.CustomerId;
            existing.RoomId = updated.RoomId;
            existing.CheckInDate = updated.CheckInDate;
            existing.CheckOutDate = updated.CheckOutDate;
            existing.TotalAmount = updated.TotalAmount;
            existing.Status = updated.Status;
            await _repository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
