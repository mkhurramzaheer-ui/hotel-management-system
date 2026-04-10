namespace HotelMobileApp.NetMAUI.Services;

public interface INavigationService
{
    Task ShowLoginAsync();
    Task ShowDashboardAsync();
    Task ShowBookRoomAsync();
    Page CreateLoginPage();
}