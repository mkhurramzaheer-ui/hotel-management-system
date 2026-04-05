using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<Billing>> GetAllAsync();
        Task<Billing?> GetByIdAsync(int id);
        Task<Billing> CreateAsync(Billing billing);
        Task UpdateAsync(int id, Billing updated);
        Task DeleteAsync(int id);
    }
}
