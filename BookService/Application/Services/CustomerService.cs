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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Customer>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Customer?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _repository.AddAsync(customer);
            return customer;
        }
        public async Task UpdateAsync(int id, Customer updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Customer not found.");
            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.Email = updated.Email;
            existing.PhoneNumber = updated.PhoneNumber;
            await _repository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
