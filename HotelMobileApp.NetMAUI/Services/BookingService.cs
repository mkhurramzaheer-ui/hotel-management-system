using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Models.Bookings;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class BookingService(IApiClient apiClient) : IBookingService
{
    private readonly IApiClient _apiClient = apiClient;

    public async Task<IReadOnlyList<BookingItem>> GetBookingsAsync(CancellationToken cancellationToken = default)
    {
        var bookings = await _apiClient.GetAsync<List<BookingItem>>(ApiRoutes.Bookings, cancellationToken: cancellationToken);
        return bookings ?? [];
    }

    public Task CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync(ApiRoutes.Bookings, request, cancellationToken: cancellationToken);
    }
}
