using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HotelMobileApp.NetMAUI.Storage;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class ApiClient(HttpClient httpClient, ITokenStorage tokenStorage) : IApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient = httpClient;
    private readonly ITokenStorage _tokenStorage = tokenStorage;

    public async Task<T?> GetAsync<T>(string uri, bool requiresAuth = true, CancellationToken cancellationToken = default)
    {
        using var request = await CreateRequestAsync(HttpMethod.Get, uri, requiresAuth);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer.DeserializeAsync<T>(responseStream, JsonOptions, cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data, bool requiresAuth = true, CancellationToken cancellationToken = default)
    {
        using var request = await CreateRequestAsync(HttpMethod.Post, uri, requiresAuth);
        request.Content = CreateJsonContent(data);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await EnsureSuccessAsync(response, cancellationToken);
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (response.Content.Headers.ContentLength is 0)
        {
            return default;
        }

        return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, JsonOptions, cancellationToken);
    }

    public async Task PostAsync<TRequest>(string uri, TRequest data, bool requiresAuth = true, CancellationToken cancellationToken = default)
    {
        using var request = await CreateRequestAsync(HttpMethod.Post, uri, requiresAuth);
        request.Content = CreateJsonContent(data);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod httpMethod, string uri, bool requiresAuth)
    {
        var request = new HttpRequestMessage(httpMethod, uri);

        if (!requiresAuth)
        {
            return request;
        }

        var token = await _tokenStorage.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return request;
    }

    private static StringContent CreateJsonContent<TRequest>(TRequest data)
    {
        var json = JsonSerializer.Serialize(data, JsonOptions);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var message = string.IsNullOrWhiteSpace(errorBody)
            ? $"API call failed with status code {(int)response.StatusCode}."
            : $"API call failed with status code {(int)response.StatusCode}: {errorBody}";

        throw new HttpRequestException(message, null, response.StatusCode);
    }
}
