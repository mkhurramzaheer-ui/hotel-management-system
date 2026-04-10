using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelMobileApp.NetMAUI.Models.Bookings;
using HotelMobileApp.NetMAUI.Services;
using System.Collections.ObjectModel;

namespace HotelMobileApp.NetMAUI.ViewModels;

public partial class DashboardViewModel(
    IAuthService authService,
    INavigationService navigationService,
    IBookingService bookingService,
    IRoomService roomService,
    ICustomerService customerService) : BaseViewModel
{
    private readonly IAuthService _authService = authService;
    private readonly INavigationService _navigationService = navigationService;
    private readonly IBookingService _bookingService = bookingService;
    private readonly IRoomService _roomService = roomService;
    private readonly ICustomerService _customerService = customerService;

    [ObservableProperty]
    public partial string WelcomeMessage { get; set; } = "Welcome back!";

    [ObservableProperty]
    public partial int TotalBookings { get; set; }

    [ObservableProperty]
    public partial int AvailableRooms { get; set; }

    [ObservableProperty]
    public partial int TotalCustomers { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<BookingItem> RecentBookings { get; set; } = [];

    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Title = "Dashboard";

            var bookingsTask = _bookingService.GetBookingsAsync();
            var roomsTask = _roomService.GetAvailableRoomsAsync();
            var customersTask = _customerService.GetCustomersAsync();

            await Task.WhenAll(bookingsTask, roomsTask, customersTask);

            var bookings = await bookingsTask;
            var availableRooms = await roomsTask;
            var customers = await customersTask;

            TotalBookings = bookings.Count;
            AvailableRooms = availableRooms.Count;
            TotalCustomers = customers.Count;

            RecentBookings = new ObservableCollection<BookingItem>(
                bookings.OrderByDescending(b => b.CreatedAt).Take(10));
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Failed to load data: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToBookRoomAsync()
    {
        await _navigationService.ShowBookRoomAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await InitializeAsync();
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await _navigationService.ShowLoginAsync();
    }
}