using HotelMobileApp.NetMAUI.ViewModels;
namespace HotelMobileApp.NetMAUI.Views;

public partial class BookRoomPage : ContentPage
{
    private readonly BookRoomViewModel _viewModel;
    public BookRoomPage(BookRoomViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}