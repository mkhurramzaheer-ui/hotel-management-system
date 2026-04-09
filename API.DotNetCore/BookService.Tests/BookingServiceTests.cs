using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Application.Services;
using Application.Interfaces;
using Application.Repository;
using Domain;

namespace BookService.Tests
{
    public class BookingServiceTests
    {
        private Mock<IBookingRepository> _repoMock;
        private BookingService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IBookingRepository>();
            _service = new BookingService(_repoMock.Object);
        }

        // ✅ Test GetAllAsync
        [Test]
        public async Task GetAllAsync_ShouldReturnAllBookings()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { Id = 1 },
                new Booking { Id = 2 }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(bookings);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        // ✅ Test GetByIdAsync
        [Test]
        public async Task GetByIdAsync_ShouldReturnBooking()
        {
            var booking = new Booking { Id = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(booking);

            var result = await _service.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        // ✅ Test CreateAsync
        [Test]
        public async Task CreateAsync_ShouldAddBooking()
        {
            var booking = new Booking { Id = 1 };

            _repoMock.Setup(r => r.AddAsync(booking)).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(booking);

            _repoMock.Verify(r => r.AddAsync(booking), Times.Once);
            Assert.That(result, Is.EqualTo(booking));
        }

        // ✅ Test UpdateAsync (Success)
        [Test]
        public async Task UpdateAsync_ShouldUpdateBooking_WhenExists()
        {
            var existing = new Booking { Id = 1 };
            var updated = new Booking
            {
                CustomerId = 2,
                RoomId = 3,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                TotalAmount = 500,
                Status = "Confirmed"
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            await _service.UpdateAsync(1, updated);

            Assert.That(existing.CustomerId, Is.EqualTo(2));
            Assert.That(existing.Status, Is.EqualTo("Confirmed"));

            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        // ❌ Test UpdateAsync (Not Found - Important)
        [Test]
        public void UpdateAsync_ShouldThrowException_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Booking?)null);

            var updated = new Booking();

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.UpdateAsync(1, updated));
        }

        // ✅ Test DeleteAsync
        [Test]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}