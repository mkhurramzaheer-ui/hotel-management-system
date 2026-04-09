using HotelAdmin.Mvc.Filters;
using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdmin.Mvc.Controllers
{
    [AuthFilter]
    public class RoomsController : Controller
    {
        private readonly IApiService _apiService;

        public RoomsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _apiService.GetAsync<List<RoomDto>>("Rooms");
            return View(rooms);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _apiService.PostAsync<object>("Rooms", model);

            return RedirectToAction("Index");
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _apiService.GetAsync<RoomDto>($"Rooms/{id}");
            return View(room);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(RoomDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _apiService.PutAsync<object>($"Rooms/{model.Id}", model);

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"Rooms/{id}");
            return RedirectToAction("Index");
        }
    }
}
