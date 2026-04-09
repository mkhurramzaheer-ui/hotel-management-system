using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Models.Auth;
using HotelMobileApp.NetMAUI.Storage;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class AuthService(IApiClient apiClient, ITokenStorage tokenStorage) : IAuthService
{
    private readonly IApiClient _apiClient = apiClient;
    private readonly ITokenStorage _tokenStorage = tokenStorage;

    public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.PostAsync<LoginRequest, LoginResponse>(
            ApiRoutes.Login,
            new LoginRequest
            {
                Username = username,
                Password = password
            },
            requiresAuth: false,
            cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(response?.Token))
        {
            return false;
        }

        await _tokenStorage.SetTokenAsync(response.Token);
        return true;
    }

    public Task LogoutAsync()
    {
        _tokenStorage.ClearToken();
        return Task.CompletedTask;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }
}
