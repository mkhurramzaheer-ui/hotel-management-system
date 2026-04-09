using HotelAdmin.Mvc.Filters;
using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdmin.Mvc.Controllers
{
    [AuthFilter]
    public class BookingsController : Controller
    {
        private readonly IApiService _apiService;

        public BookingsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var bookings = await _apiService.GetAsync<List<BookingDto>>("Bookings");
            return View(bookings);
        }

        // CREATE
        public async Task<IActionResult> Create()
        {
            var vm = new BookingViewModel
            {
                Customers = await _apiService.GetAsync<List<CustomerDto>>("Customers"),
                Rooms = await _apiService.GetAsync<List<RoomDto>>("Rooms")
            };

            // 🔥 Load existing bookings
            ViewBag.ExistingBookings = await _apiService.GetAsync<List<BookingDto>>("Bookings");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Customers = await _apiService.GetAsync<List<CustomerDto>>("Customers");
                model.Rooms = await _apiService.GetAsync<List<RoomDto>>("Rooms");
                return View(model);
            }

            var dto = new CreateBookingDto
            {
                CustomerId = model.CustomerId,
                RoomId = model.RoomId,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                TotalAmount = model.TotalAmount
            };

            await _apiService.PostAsync<object>("Bookings", dto);

            return RedirectToAction("Index");
        }
       
        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"Bookings/{id}");
            return RedirectToAction("Index");
        }
    }
}
