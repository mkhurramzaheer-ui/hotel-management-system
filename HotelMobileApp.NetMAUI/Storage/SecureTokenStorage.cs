namespace HotelMobileApp.NetMAUI.Storage;

public sealed class SecureTokenStorage : ITokenStorage
{
    private const string TokenKey = "hotel_booking_jwt_token";

    public Task<string?> GetTokenAsync()
    {
        return SecureStorage.Default.GetAsync(TokenKey);
    }

    public Task SetTokenAsync(string token)
    {
        return SecureStorage.Default.SetAsync(TokenKey, token);
    }

    public void ClearToken()
    {
        SecureStorage.Default.Remove(TokenKey);
    }
}
