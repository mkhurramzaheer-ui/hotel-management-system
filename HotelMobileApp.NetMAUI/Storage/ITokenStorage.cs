namespace HotelMobileApp.NetMAUI.Storage;

public interface ITokenStorage
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token);
    void ClearToken();
}
