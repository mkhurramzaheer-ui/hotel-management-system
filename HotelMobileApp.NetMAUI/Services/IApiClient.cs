namespace HotelMobileApp.NetMAUI.Services;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string uri, bool requiresAuth = true, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data, bool requiresAuth = true, CancellationToken cancellationToken = default);
    Task PostAsync<TRequest>(string uri, TRequest data, bool requiresAuth = true, CancellationToken cancellationToken = default);
}
