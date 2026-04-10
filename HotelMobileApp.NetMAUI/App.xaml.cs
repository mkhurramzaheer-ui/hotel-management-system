using Microsoft.Extensions.DependencyInjection;

namespace HotelMobileApp.NetMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var navigationService = Handler?.MauiContext?.Services.GetRequiredService<Services.INavigationService>();
            var rootPage = navigationService?.CreateLoginPage() ?? new ContentPage();
            return new Window(rootPage);
        }
    }
}
