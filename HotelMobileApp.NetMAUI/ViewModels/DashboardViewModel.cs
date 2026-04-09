using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelMobileApp.NetMAUI.Services;

namespace HotelMobileApp.NetMAUI.ViewModels;

public partial class DashboardViewModel(IAuthService authService, INavigationService navigationService) : BaseViewModel
{
    private readonly IAuthService _authService = authService;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    public partial string WelcomeMessage { get; set; } = "Welcome to the hotel dashboard.";

    [ObservableProperty]
    public partial string SummaryMessage { get; set; } = "Next we will load total bookings, available rooms, and customers from your API.";

    public async Task InitializeAsync()
    {
        Title = "Dashboard";
        ErrorMessage = string.Empty;
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await _navigationService.ShowLoginAsync();
    }
}
