using HotelMobileApp.NetMAUI.Views;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class NavigationService(IServiceProvider serviceProvider) : INavigationService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task ShowLoginAsync()
    {
        SetRootPage(CreateLoginPage());
        return Task.CompletedTask;
    }

    public Task ShowDashboardAsync()
    {
        var appShell = _serviceProvider.GetRequiredService<AppShell>();
        SetRootPage(appShell);
        return Task.CompletedTask;
    }

    public Page CreateLoginPage()
    {
        var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
        return new NavigationPage(loginPage)
        {
            BarBackgroundColor = Color.FromArgb("#F6F1E9"),
            BarTextColor = Color.FromArgb("#1F3A5F")
        };
    }

    private static void SetRootPage(Page page)
    {
        var currentWindow = Application.Current?.Windows.FirstOrDefault();
        if (currentWindow is not null)
        {
            currentWindow.Page = page;
        }
    }
}
