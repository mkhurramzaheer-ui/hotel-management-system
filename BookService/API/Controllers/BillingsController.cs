using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillingsController : ControllerBase
{
    private readonly IBillingService _service;

    public BillingsController(IBillingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var biilings = await _service.GetAllAsync();
        var result = biilings.Select(b => new BillingDto
        {
            Id = b.Id,
            BookingId = b.BookingId,
            Amount = b.Amount,
            PaymentStatus = b.PaymentStatus,
            BillingDate = b.BillingDate,

            Booking = new BookingDto
            {
                Id = b.Booking.Id,
                CustomerId = b.Booking.CustomerId,
                RoomId = b.Booking.RoomId,
                CheckInDate = b.Booking.CheckInDate,
                CheckOutDate = b.Booking.CheckOutDate,
                TotalAmount = b.Booking.TotalAmount,
                Status = b.Booking.Status,

                Customer = new CustomerDto
                {
                    Id = b.Booking.Customer.Id,
                    FirstName = b.Booking.Customer.FirstName,
                    LastName = b.Booking.Customer.LastName
                },

                Room = new RoomDto
                {
                    Id = b.Booking.Room.Id,
                    RoomNumber = b.Booking.Room.RoomNumber,
                    Type = b.Booking.Room.Type
                }
            }
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBillingDto billing)
    {
        var newBilling = new Billing
        {
            BookingId = billing.BookingId,
            Amount = billing.Amount,
            PaymentStatus = billing.PaymentStatus,
            BillingDate = DateTime.UtcNow
        };
        var created = await _service.CreateAsync(newBilling);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBillingDto billing)
    {
        var newBilling = new Billing
        {
            BookingId = billing.BookingId,
            Amount = billing.Amount,
            PaymentStatus = billing.PaymentStatus,
            BillingDate = DateTime.UtcNow
        };
        await _service.UpdateAsync(id, newBilling);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
