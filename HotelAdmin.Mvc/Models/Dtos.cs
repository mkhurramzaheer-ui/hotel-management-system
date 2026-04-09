namespace HotelAdmin.Mvc.Models
{
    public class RoomDto { public int Id { get; set; } public string RoomNumber { get; set; } = string.Empty; public string Type { get; set; } = string.Empty; public decimal PricePerNight { get; set; } public bool IsAvailable { get; set; } = true; }
    public class CreateRoomDto { public string RoomNumber { get; set; } = string.Empty; public string Type { get; set; } = string.Empty; public decimal PricePerNight { get; set; } public bool IsAvailable { get; set; } = true; }
    public class CustomerDto { public int Id { get; set; } public string FirstName { get; set; } = string.Empty; public string LastName { get; set; } = string.Empty; public string Email { get; set; } = string.Empty; public string PhoneNumber { get; set; } = string.Empty; public string FullName => $"{FirstName} {LastName}"; }
    public class CreateCustomerDto { public string FirstName { get; set; } = string.Empty; public string LastName { get; set; } = string.Empty; public string Email { get; set; } = string.Empty; public string PhoneNumber { get; set; } = string.Empty; }
    public class BookingDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; public DateTime CreatedAt { get; set; }
        public CustomerDto? Customer { get; set; }
        public RoomDto? Room { get; set; }
    }
    public class BookingViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public int RoomId { get; set; }

        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        public decimal TotalAmount { get; set; }

        public List<CustomerDto> Customers { get; set; } = new();
        public List<RoomDto> Rooms { get; set; } = new();
    }
    public class CreateBookingDto { public int CustomerId { get; set; } public int RoomId { get; set; } public DateTime CheckInDate { get; set; } = DateTime.Today; public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1); public decimal TotalAmount { get; set; } } 
    public class BillingDto { public int Id { get; set; } public int BookingId { get; set; } public decimal Amount { get; set; } public string PaymentStatus { get; set; } = "Unpaid"; public DateTime BillingDate { get; set; } public BookingDto? Booking { get; set; } }
    public class CreateBillingDto { public int BookingId { get; set; } public decimal Amount { get; set; } public string PaymentStatus { get; set; } = "Unpaid"; }
}
