using Application.Interfaces;
using Application.Repository;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _repository;
        public BillingService(IBillingRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Billing>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Billing?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Billing> CreateAsync(Billing billing)
        {
            await _repository.AddAsync(billing);
            return billing;
        }
        public async Task UpdateAsync(int id, Billing updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Billing not found.");
            existing.Amount = updated.Amount;
            existing.PaymentStatus = updated.PaymentStatus;
            existing.BillingDate = updated.BillingDate;
            await _repository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
