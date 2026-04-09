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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;
        public RoomService(IRoomRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Room>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Room?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Room> CreateAsync(Room room)
        {
            await _repository.AddAsync(room);
            return room;
        }
        public async Task UpdateAsync(int id, Room updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Room not found.");
            existing.RoomNumber = updated.RoomNumber;
            existing.PricePerNight = updated.PricePerNight;
            existing.Type = updated.Type;
            existing.IsAvailable = updated.IsAvailable;
            await _repository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
