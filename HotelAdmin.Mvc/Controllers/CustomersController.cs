using HotelAdmin.Mvc.Filters;
using HotelAdmin.Mvc.Interfaces;
using HotelAdmin.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelAdmin.Mvc.Controllers
{
    [AuthFilter]
    public class CustomersController : Controller
    {
        private readonly IApiService _apiService;

        public CustomersController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _apiService.GetAsync<List<CustomerDto>>("Customers");
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _apiService.PostAsync<object>("Customers", model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _apiService.GetAsync<CustomerDto>($"Customers/{id}");
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _apiService.PutAsync<object>($"Customers/{model.Id}", model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"Customers/{id}");
            return RedirectToAction("Index");
        }
    }
}
