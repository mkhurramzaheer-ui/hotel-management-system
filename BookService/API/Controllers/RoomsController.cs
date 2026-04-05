using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController(IRoomService service) : ControllerBase
    {
        private readonly IRoomService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _service.GetByIdAsync(id);
            return room is null ? NotFound() : Ok(room);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            var created = await _service.CreateAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Room room)
        {
            await _service.UpdateAsync(id, room);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
