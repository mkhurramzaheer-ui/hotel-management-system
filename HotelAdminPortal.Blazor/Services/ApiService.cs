using HotelAdmin.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace HotelAdmin.Services
{
    // ─── Request / Response mediator contracts ───────────────────────────
    public interface IRequest<TResponse> { }

    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }

    public interface IMediator
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
    }

    // ─── Simple Mediator implementation ──────────────────────────────────
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;
        public Mediator(IServiceProvider provider) => _provider = provider;

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            dynamic handler = _provider.GetRequiredService(handlerType);
            return handler.HandleAsync((dynamic)request);
        }
    }

    // ─── HTTP client helper ───────────────────────────────────────────────
    public class ApiHttpClient
    {
        private readonly HttpClient _http;
        private readonly AuthStateService _auth;

        public ApiHttpClient(HttpClient http, AuthStateService auth)
        {
            _http = http;
            _auth = auth;
        }

        private void SetAuth()
        {
            if (!string.IsNullOrEmpty(_auth.Token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _auth.Token);
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            SetAuth();
            return await _http.GetFromJsonAsync<T>(url);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T body)
        {
            SetAuth();
            return await _http.PostAsJsonAsync(url, body);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T body)
        {
            SetAuth();
            return await _http.PutAsJsonAsync(url, body);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            SetAuth();
            return await _http.DeleteAsync(url);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  ROOM Requests & Handlers
    // ═══════════════════════════════════════════════════════════════════════
    public record GetRoomsRequest() : IRequest<List<Room>>;
    public record CreateRoomRequest(Room Room) : IRequest<bool>;
    public record UpdateRoomRequest(int Id, Room Room) : IRequest<bool>;
    public record DeleteRoomRequest(int Id) : IRequest<bool>;

    public class GetRoomsHandler : IRequestHandler<GetRoomsRequest, List<Room>>
    {
        private readonly ApiHttpClient _api;
        public GetRoomsHandler(ApiHttpClient api) => _api = api;
        public async Task<List<Room>> HandleAsync(GetRoomsRequest request)
            => await _api.GetAsync<List<Room>>("api/Rooms") ?? new();
    }

    public class CreateRoomHandler : IRequestHandler<CreateRoomRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public CreateRoomHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(CreateRoomRequest request)
        {
            var res = await _api.PostAsync("api/Rooms", request.Room);
            return res.IsSuccessStatusCode;
        }
    }

    public class UpdateRoomHandler : IRequestHandler<UpdateRoomRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public UpdateRoomHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(UpdateRoomRequest request)
        {
            var res = await _api.PutAsync($"api/Rooms/{request.Id}", request.Room);
            return res.IsSuccessStatusCode;
        }
    }

    public class DeleteRoomHandler : IRequestHandler<DeleteRoomRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public DeleteRoomHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(DeleteRoomRequest request)
        {
            var res = await _api.DeleteAsync($"api/Rooms/{request.Id}");
            return res.IsSuccessStatusCode;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  CUSTOMER Requests & Handlers
    // ═══════════════════════════════════════════════════════════════════════
    public record GetCustomersRequest() : IRequest<List<CustomerDto>>;
    public record CreateCustomerRequest(Customer Customer) : IRequest<bool>;
    public record UpdateCustomerRequest(int Id, Customer Customer) : IRequest<bool>;
    public record DeleteCustomerRequest(int Id) : IRequest<bool>;

    public class GetCustomersHandler : IRequestHandler<GetCustomersRequest, List<CustomerDto>>
    {
        private readonly ApiHttpClient _api;
        public GetCustomersHandler(ApiHttpClient api) => _api = api;
        public async Task<List<CustomerDto>> HandleAsync(GetCustomersRequest request)
            => await _api.GetAsync<List<CustomerDto>>("api/Customers") ?? new();
    }

    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public CreateCustomerHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(CreateCustomerRequest request)
        {
            var res = await _api.PostAsync("api/Customers", request.Customer);
            return res.IsSuccessStatusCode;
        }
    }

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public UpdateCustomerHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(UpdateCustomerRequest request)
        {
            var res = await _api.PutAsync($"api/Customers/{request.Id}", request.Customer);
            return res.IsSuccessStatusCode;
        }
    }

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public DeleteCustomerHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(DeleteCustomerRequest request)
        {
            var res = await _api.DeleteAsync($"api/Customers/{request.Id}");
            return res.IsSuccessStatusCode;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  BOOKING Requests & Handlers
    // ═══════════════════════════════════════════════════════════════════════
    public record GetBookingsRequest() : IRequest<List<BookingDto>>;
    public record CreateBookingRequestCmd(CreateBookingRequest Booking) : IRequest<bool>;
    public record DeleteBookingRequest(int Id) : IRequest<bool>;

    public class GetBookingsHandler : IRequestHandler<GetBookingsRequest, List<BookingDto>>
    {
        private readonly ApiHttpClient _api;
        public GetBookingsHandler(ApiHttpClient api) => _api = api;
        public async Task<List<BookingDto>> HandleAsync(GetBookingsRequest request)
            => await _api.GetAsync<List<BookingDto>>("api/Bookings") ?? new();
    }

    public class CreateBookingHandler : IRequestHandler<CreateBookingRequestCmd, bool>
    {
        private readonly ApiHttpClient _api;
        public CreateBookingHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(CreateBookingRequestCmd request)
        {
            var res = await _api.PostAsync("api/Bookings", request.Booking);
            return res.IsSuccessStatusCode;
        }
    }

    public class DeleteBookingHandler : IRequestHandler<DeleteBookingRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public DeleteBookingHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(DeleteBookingRequest request)
        {
            var res = await _api.DeleteAsync($"api/Bookings/{request.Id}");
            return res.IsSuccessStatusCode;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  BILLING Requests & Handlers
    // ═══════════════════════════════════════════════════════════════════════
    public record GetBillingsRequest() : IRequest<List<BillingDto>>;
    public record CreateBillingRequest(CreateBillingDto Billing) : IRequest<bool>;
    public record DeleteBillingRequest(int Id) : IRequest<bool>;

    public class GetBillingsHandler : IRequestHandler<GetBillingsRequest, List<BillingDto>>
    {
        private readonly ApiHttpClient _api;
        public GetBillingsHandler(ApiHttpClient api) => _api = api;
        public async Task<List<BillingDto>> HandleAsync(GetBillingsRequest request)
            => await _api.GetAsync<List<BillingDto>>("api/Billings") ?? new();
    }

    public class CreateBillingHandler : IRequestHandler<CreateBillingRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public CreateBillingHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(CreateBillingRequest request)
        {
            var res = await _api.PostAsync("api/Billings", request.Billing);
            return res.IsSuccessStatusCode;
        }
    }

    public class DeleteBillingHandler : IRequestHandler<DeleteBillingRequest, bool>
    {
        private readonly ApiHttpClient _api;
        public DeleteBillingHandler(ApiHttpClient api) => _api = api;
        public async Task<bool> HandleAsync(DeleteBillingRequest request)
        {
            var res = await _api.DeleteAsync($"api/Billings/{request.Id}");
            return res.IsSuccessStatusCode;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    //  AUTH Requests & Handlers
    // ═══════════════════════════════════════════════════════════════════════
    public record LoginRequestCmd(LoginRequest Credentials) : IRequest<LoginResponse>;

    public class LoginHandler : IRequestHandler<LoginRequestCmd, LoginResponse>
    {
        private readonly ApiHttpClient _api;
        public LoginHandler(ApiHttpClient api) => _api = api;
        public async Task<LoginResponse> HandleAsync(LoginRequestCmd request)
        {
            var res = await _api.PostAsync("api/Auth/login", request.Credentials);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                // Try to deserialize token from response
                try
                {
                    var response = JsonSerializer.Deserialize<LoginResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return response ?? new LoginResponse { Success = true, Token = content };
                }
                catch
                {
                    return new LoginResponse { Success = true, Token = content };
                }
            }
            return new LoginResponse { Success = false, Message = "Invalid credentials" };
        }
    }
}
