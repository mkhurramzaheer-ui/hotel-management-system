namespace HotelMobileApp.NetMAUI.Models.Auth;

public sealed class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
