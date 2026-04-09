using Domain;

namespace Application.Repository;

public interface IBillingRepository
{
    Task<IEnumerable<Billing>> GetAllAsync();
    Task<Billing?> GetByIdAsync(int id);
    Task AddAsync(Billing billing);
    Task UpdateAsync(Billing billing);
    Task DeleteAsync(int id);
}
