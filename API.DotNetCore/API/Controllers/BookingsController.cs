using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService service) : ControllerBase
{
    private readonly IBookingService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _service.GetAllAsync();
        var result = bookings.Select(b => new BookingDto
        {
            Id = b.Id,
            CustomerId = b.CustomerId,
            RoomId = b.RoomId,
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            TotalAmount = b.TotalAmount,
            Status = b.Status,
            CreatedAt = b.CreatedAt,

            Customer = new CustomerDto
            {
                Id = b.Customer.Id,
                FirstName = b.Customer.FirstName,
                LastName = b.Customer.LastName,
                Email = b.Customer.Email,
                PhoneNumber = b.Customer.PhoneNumber
            },

            Room = new RoomDto
            {
                Id = b.Room.Id,
                RoomNumber = b.Room.RoomNumber,
                Type = b.Room.Type,
                PricePerNight = b.Room.PricePerNight
            }
        }).ToList();

        return Ok(result);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _service.GetByIdAsync(id);
        return booking == null ? NotFound() : Ok(booking);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
    {
        var booking = new Booking
        {
            CustomerId = dto.CustomerId,
            RoomId = dto.RoomId,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            TotalAmount = dto.TotalAmount,
            Status = "Confirmed"
        };
        await _service.CreateAsync(booking);
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBookingDto dto)
    {
        var booking = new Booking
        {
            CustomerId = dto.CustomerId,
            RoomId = dto.RoomId,
            CheckInDate = dto.CheckInDate,
            CheckOutDate = dto.CheckOutDate,
            TotalAmount = dto.TotalAmount,
            Status = "Confirmed"
        };
        await _service.UpdateAsync(id, booking);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
