using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BookDbContext(DbContextOptions<BookDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Billing> Billings => Set<Billing>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customer 1..* Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Room 1..* Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
            // Booking 1..1 Billing
            modelBuilder.Entity<Billing>()
                .HasOne(b => b.Booking)
                .WithOne(bk => bk.Billing)
                .HasForeignKey<Billing>(b => b.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        // ---------- Data Seed ----------
        public static void SeedData(BookDbContext context)
        {
            context.Database.EnsureCreated();
            // Customers
            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer { FirstName = "Alice", LastName = "Johnson", Email = "alice@example.com", PhoneNumber = "111-1111" },
                    new Customer { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com", PhoneNumber = "222-2222" }
                );
                context.SaveChanges();
            }
            // Rooms
            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new Room { RoomNumber = "101", Type = "Deluxe", PricePerNight = 150, IsAvailable = true },
                    new Room { RoomNumber = "102", Type = "Standard", PricePerNight = 100, IsAvailable = true },
                    new Room { RoomNumber = "201", Type = "Suite", PricePerNight = 250, IsAvailable = true }
                );
                context.SaveChanges();
            }
            // Bookings
            if (!context.Bookings.Any())
            {
                var customer1 = context.Customers.First();
                var room1 = context.Rooms.First();
                var booking = new Booking
                {
                    CustomerId = customer1.Id,
                    RoomId = room1.Id,
                    CheckInDate = DateTime.UtcNow.Date,
                    CheckOutDate = DateTime.UtcNow.Date.AddDays(2),
                    TotalAmount = room1.PricePerNight * 2,
                    Status = "Confirmed"
                };
                context.Bookings.Add(booking);
                context.SaveChanges();
                // Billing for that booking
                context.Billings.Add(new Billing
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalAmount,
                    PaymentStatus = "Paid",
                    BillingDate = DateTime.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
}
