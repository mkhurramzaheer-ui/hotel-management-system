using System.Text.Json.Serialization;

namespace HotelMobileApp.NetMAUI.Models.Auth;

public sealed class LoginResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
