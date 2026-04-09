namespace HotelMobileApp.NetMAUI.Models.Rooms;

public sealed class RoomItem
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }

    public string DisplayName => $"{RoomNumber} - {Type}";
}
