using HotelMobileApp.NetMAUI.Models.Bookings;

namespace HotelMobileApp.NetMAUI.Services;

public interface IBookingService
{
    Task<IReadOnlyList<BookingItem>> GetBookingsAsync(CancellationToken cancellationToken = default);
    Task CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);
}
