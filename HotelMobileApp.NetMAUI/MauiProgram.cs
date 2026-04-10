using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

using HotelMobileApp.NetMAUI.Configuration;
using HotelMobileApp.NetMAUI.Services;
using HotelMobileApp.NetMAUI.Storage;
using HotelMobileApp.NetMAUI.ViewModels;
using HotelMobileApp.NetMAUI.Views;
using MediatR;

namespace HotelMobileApp.NetMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var apiBaseUrl = GetApiBaseUrl();

        builder.Services.AddSingleton(new ApiSettings
        {
            BaseUrl = apiBaseUrl
        });

        builder.Services.AddSingleton<ITokenStorage, SecureTokenStorage>();

        builder.Services.AddSingleton(serviceProvider =>
        {
            var apiSettings = serviceProvider.GetRequiredService<ApiSettings>();

            return new HttpClient
            {
                BaseAddress = new Uri(apiSettings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        });

        builder.Services.AddSingleton<IApiClient, ApiClient>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IRoomService, RoomService>();
        builder.Services.AddSingleton<ICustomerService, CustomerService>();
        builder.Services.AddSingleton<IBookingService, BookingService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MauiProgram).Assembly);
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static string GetApiBaseUrl()
    {
#if ANDROID
        return "http://10.0.2.2:8080/";
#else
        return "http://localhost:8080/";
#endif
    }
}
