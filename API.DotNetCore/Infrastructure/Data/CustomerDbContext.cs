using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            });
        }
    }
}
