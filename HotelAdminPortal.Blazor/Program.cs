using Blazored.LocalStorage;
using HotelAdmin.Services;
using HotelAdminPortal.Blazor.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Razor / Blazor ───────────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

// ── MudBlazor ────────────────────────────────────────────────────────────
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
// ── HTTP Client ──────────────────────────────────────────────────────────
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001/";
builder.Services.AddHttpClient<ApiHttpClient>(client =>
{
    client.BaseAddress = new Uri(apiBase);
});

// ── Auth State ───────────────────────────────────────────────────────────
builder.Services.AddScoped<AuthStateService>();

// ── Mediator ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IMediator, Mediator>();

// ── Request Handlers ─────────────────────────────────────────────────────
builder.Services.AddScoped<IRequestHandler<GetRoomsRequest,     List<HotelAdmin.Models.Room>>,     GetRoomsHandler>();
builder.Services.AddScoped<IRequestHandler<CreateRoomRequest,   bool>,                              CreateRoomHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateRoomRequest,   bool>,                              UpdateRoomHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteRoomRequest,   bool>,                              DeleteRoomHandler>();

builder.Services.AddScoped<IRequestHandler<GetCustomersRequest,     List<HotelAdmin.Models.CustomerDto>>, GetCustomersHandler>();
builder.Services.AddScoped<IRequestHandler<CreateCustomerRequest,   bool>,                                CreateCustomerHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateCustomerRequest,   bool>,                                UpdateCustomerHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteCustomerRequest,   bool>,                                DeleteCustomerHandler>();

builder.Services.AddScoped<IRequestHandler<GetBookingsRequest,      List<HotelAdmin.Models.Booking>>, GetBookingsHandler>();
builder.Services.AddScoped<IRequestHandler<CreateBookingRequestCmd, bool>,                               CreateBookingHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteBookingRequest,    bool>,                               DeleteBookingHandler>();

builder.Services.AddScoped<IRequestHandler<GetBillingsRequest,    List<HotelAdmin.Models.BillingDto>>, GetBillingsHandler>();
builder.Services.AddScoped<IRequestHandler<CreateBillingRequest,  bool>,                                CreateBillingHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteBillingRequest,  bool>,                                DeleteBillingHandler>();

builder.Services.AddScoped<IRequestHandler<LoginRequestCmd, HotelAdmin.Models.LoginResponse>, LoginHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
