using HotelAdmin.Mvc.Filters;
using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HotelAdmin.Mvc.Controllers;

[AuthFilter]
public class HomeController : Controller
{
    private readonly IApiService _apiService;

    public HomeController(IApiService apiService)
    {
        _apiService = apiService;
    }
    public async Task<IActionResult> Index()
    {
        var rooms = await _apiService.GetAsync<List<RoomDto>>("Rooms");
        var customers = await _apiService.GetAsync<List<CustomerDto>>("Customers");
        var bookings = await _apiService.GetAsync<List<BookingDto>>("Bookings");
        var billings = await _apiService.GetAsync<List<BillingDto>>("Billings");

        var vm = new DashboardViewModel
        {
            TotalRooms = rooms.Count,
            TotalCustomers = customers.Count,
            TotalBookings = bookings.Count,
            TotalRevenue = billings.Where(b => b.PaymentStatus == "Paid").Sum(b => b.Amount)
        };
        ViewBag.RevenueData = billings
    .Where(b => b.PaymentStatus == "Paid")
    .GroupBy(b => b.BillingDate.Date)
    .Select(g => new {
        date = g.Key.ToString("yyyy-MM-dd"),
        total = g.Sum(x => x.Amount)
    })
    .ToList();
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
