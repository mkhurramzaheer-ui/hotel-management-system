namespace HotelMobileApp.NetMAUI.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
}
