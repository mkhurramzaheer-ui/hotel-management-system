namespace HotelAdmin.Mvc.Services
{
    using HotelAdmin.Mvc.Interfaces;
    using HotelAdmin.Mvc.Models;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            AddJwtToken();

            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            AddJwtToken();

            var json = JsonSerializer.Serialize(data);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Token is invalid or expired.");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API Error: {response.StatusCode} - {responseContent}");

            if (string.IsNullOrWhiteSpace(responseContent))
                return default!;

            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            AddJwtToken();

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            // ✅ Handle errors
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API Error: {response.StatusCode} - {result}");

            // ✅ Handle empty response (MOST IMPORTANT FIX)
            if (string.IsNullOrWhiteSpace(result))
                return default!;

            // ✅ Deserialize only if data exists
            return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            AddJwtToken();

            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }
    }
}
