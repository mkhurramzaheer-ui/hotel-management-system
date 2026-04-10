using HotelMobileApp.NetMAUI.Common;
using HotelMobileApp.NetMAUI.Models.Customers;

namespace HotelMobileApp.NetMAUI.Services;

public sealed class CustomerService(IApiClient apiClient) : ICustomerService
{
    private readonly IApiClient _apiClient = apiClient;

    public async Task<IReadOnlyList<CustomerItem>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _apiClient.GetAsync<List<CustomerItem>>(ApiRoutes.Customers, requiresAuth: false, cancellationToken: cancellationToken);
        return customers ?? [];
    }
}
