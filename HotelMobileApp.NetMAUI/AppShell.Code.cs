using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace HotelMobileApp.NetMAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register the BookRoom route for Shell navigation
        Routing.RegisterRoute(AppRoutes.BookRoom, typeof(BookRoomPage));

        var dashboardPage = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<DashboardPage>();
        if (dashboardPage is null)
        {
            return;
        }

        Items.Add(new ShellContent
        {
            Route = AppRoutes.Dashboard,
            Title = "Dashboard",
            ContentTemplate = new DataTemplate(() => dashboardPage)
        });
    }
}