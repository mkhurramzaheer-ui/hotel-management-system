using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelMobileApp.NetMAUI.Models.Bookings;
using HotelMobileApp.NetMAUI.Models.Customers;
using HotelMobileApp.NetMAUI.Models.Rooms;
using HotelMobileApp.NetMAUI.Services;
using System.Collections.ObjectModel;

namespace HotelMobileApp.NetMAUI.ViewModels;

public partial class BookRoomViewModel(
    IBookingService bookingService,
    IRoomService roomService,
    ICustomerService customerService,
    INavigationService navigationService) : BaseViewModel
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IRoomService _roomService = roomService;
    private readonly ICustomerService _customerService = customerService;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    public partial ObservableCollection<CustomerItem> Customers { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<RoomItem> AvailableRooms { get; set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalAmount))]
    [NotifyPropertyChangedFor(nameof(CanBook))]
    public partial CustomerItem? SelectedCustomer { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalAmount))]
    [NotifyPropertyChangedFor(nameof(CanBook))]
    public partial RoomItem? SelectedRoom { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalAmount))]
    [NotifyPropertyChangedFor(nameof(CanBook))]
    public partial DateTime CheckInDate { get; set; } = DateTime.Today;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalAmount))]
    [NotifyPropertyChangedFor(nameof(CanBook))]
    public partial DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    [ObservableProperty]
    public partial bool IsSuccess { get; set; }

    // Exposed for DatePicker MinimumDate binding — avoids x:Static in XAML
    public DateTime MinDate => DateTime.Today;

    public decimal TotalAmount
    {
        get
        {
            if (SelectedRoom is null || CheckOutDate <= CheckInDate)
                return 0;

            var nights = (int)(CheckOutDate - CheckInDate).TotalDays;
            return SelectedRoom.PricePerNight * nights;
        }
    }

    public bool CanBook =>
        SelectedCustomer is not null &&
        SelectedRoom is not null &&
        CheckOutDate > CheckInDate &&
        CheckInDate >= DateTime.Today;

    public int NightsCount =>
        CheckOutDate > CheckInDate
            ? (int)(CheckOutDate - CheckInDate).TotalDays
            : 0;

    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Title = "Book a Room";

            var customersTask = _customerService.GetCustomersAsync();
            var roomsTask = _roomService.GetAvailableRoomsAsync();

            await Task.WhenAll(customersTask, roomsTask);

            var customers = await customersTask;
            var rooms = await roomsTask;

            Customers = new ObservableCollection<CustomerItem>(customers);
            AvailableRooms = new ObservableCollection<RoomItem>(rooms);
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

    partial void OnCheckInDateChanged(DateTime value)
    {
        if (CheckOutDate <= value)
            CheckOutDate = value.AddDays(1);

        OnPropertyChanged(nameof(NightsCount));
    }

    partial void OnCheckOutDateChanged(DateTime value)
    {
        OnPropertyChanged(nameof(NightsCount));
    }

    partial void OnSelectedRoomChanged(RoomItem? value)
    {
        OnPropertyChanged(nameof(NightsCount));
    }

    [RelayCommand(CanExecute = nameof(CanBook))]
    private async Task BookAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var request = new CreateBookingRequest
            {
                CustomerId = SelectedCustomer!.Id,
                RoomId = SelectedRoom!.Id,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                TotalAmount = TotalAmount,
                Status = "Confirmed"
            };

            await _bookingService.CreateBookingAsync(request);
            IsSuccess = true;

            await Application.Current!.Windows[0].Page!.DisplayAlert(
                "Booking Confirmed ✓",
                $"Room {SelectedRoom.RoomNumber} booked for {SelectedCustomer.FullName}.\n{NightsCount} night(s) · ${TotalAmount:F2} total",
                "Done");

            await _navigationService.ShowDashboardAsync();
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = $"Booking failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.ShowDashboardAsync();
    }
}