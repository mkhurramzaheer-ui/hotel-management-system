using HotelMobileApp.NetMAUI.Models.Customers;
using HotelMobileApp.NetMAUI.Models.Rooms;

namespace HotelMobileApp.NetMAUI.Models.Bookings;

public sealed class BookingItem
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public CustomerItem? Customer { get; set; }
    public RoomItem? Room { get; set; }
}
