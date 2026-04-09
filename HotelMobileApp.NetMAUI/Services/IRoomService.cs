using HotelMobileApp.NetMAUI.Models.Rooms;

namespace HotelMobileApp.NetMAUI.Services;

public interface IRoomService
{
    Task<IReadOnlyList<RoomItem>> GetRoomsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoomItem>> GetAvailableRoomsAsync(CancellationToken cancellationToken = default);
}
