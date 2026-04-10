using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelMobileApp.NetMAUI.Features.Auth;
using HotelMobileApp.NetMAUI.Services;
using MediatR;

namespace HotelMobileApp.NetMAUI.ViewModels;

public partial class LoginViewModel(IMediator mediator, INavigationService navigationService) : BaseViewModel
{
    private readonly IMediator _mediator = mediator;
    private readonly INavigationService _navigationService = navigationService;

    [ObservableProperty]
    public partial string Username { get; set; } = "admin";

    [ObservableProperty]
    public partial string Password { get; set; } = "password";

    public async Task InitializeAsync()
    {
        Title = "Login";
        ErrorMessage = string.Empty;
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var result = await _mediator.Send(new LoginCommand(Username, Password));
            if (!result.IsSuccess)
            {
                ErrorMessage = result.ErrorMessage;
                return;
            }

            await _navigationService.ShowDashboardAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
