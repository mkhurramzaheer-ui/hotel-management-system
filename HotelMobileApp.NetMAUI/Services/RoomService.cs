using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Models.Rooms;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class RoomService(IApiClient apiClient) : IRoomService
{
    private readonly IApiClient _apiClient = apiClient;

    public async Task<IReadOnlyList<RoomItem>> GetRoomsAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _apiClient.GetAsync<List<RoomItem>>(ApiRoutes.Rooms, cancellationToken: cancellationToken);
        return rooms ?? [];
    }

    public async Task<IReadOnlyList<RoomItem>> GetAvailableRoomsAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await GetRoomsAsync(cancellationToken);
        return rooms.Where(room => room.IsAvailable).ToList();
    }
}
