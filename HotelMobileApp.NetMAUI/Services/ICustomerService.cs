using HotelMobileApp.NetMAUI.Models.Customers;

namespace HotelMobileApp.NetMAUI.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerItem>> GetCustomersAsync(CancellationToken cancellationToken = default);
}
