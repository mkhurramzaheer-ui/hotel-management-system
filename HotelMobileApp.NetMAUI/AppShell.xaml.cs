namespace HotelMobileApp.NetMAUI
using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Views;

namespace HotelMobileApp.NetMAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

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
