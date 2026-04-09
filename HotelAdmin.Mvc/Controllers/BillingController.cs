using HotelAdmin.Mvc.Filters;
using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdmin.Mvc.Controllers
{
    [AuthFilter]
    public class BillingController : Controller
    {
        private readonly IApiService _apiService;

        public BillingController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var billings = await _apiService.GetAsync<List<BillingDto>>("Billings");
            return View(billings);
        }

        // CREATE
        public async Task<IActionResult> Create()
        {
            var bookings = await _apiService.GetAsync<List<BookingDto>>("Bookings");

            ViewBag.Bookings = bookings;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBillingDto model)
        {
            var existing = await _apiService.GetAsync<List<BillingDto>>("Billings");

            var found = existing.FirstOrDefault(x => x.BookingId == model.BookingId);

            if (found != null)
            {
                // 🔥 Delete instead
                await _apiService.DeleteAsync($"Billings/{found.Id}");
            }
            await _apiService.PostAsync<object>("Billings", model);

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"Billings/{id}");
            return RedirectToAction("Index");
        }
    }
}
