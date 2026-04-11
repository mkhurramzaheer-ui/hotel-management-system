namespace HotelAdmin.Models
{
    // ─── Auth ───────────────────────────────────────────────────────────
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ─── Room ────────────────────────────────────────────────────────────
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class RoomDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
    }

    // ─── Customer ────────────────────────────────────────────────────────
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
    }

    // ─── Booking ─────────────────────────────────────────────────────────
    public class BookingDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public CustomerDto? Customer { get; set; }
        public RoomDto? Room { get; set; }
    }

    public class CreateBookingRequest
    {
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
        public double TotalAmount { get; set; }
        public string Status { get; set; } = "Confirmed";
    }

    // ─── Billing ─────────────────────────────────────────────────────────
    public class CreateBillingDto
    {
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }

    public class BillingDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime BillingDate { get; set; }
        public BookingDto? Booking { get; set; }
    }

    // ─── Dashboard ───────────────────────────────────────────────────────
    public class DashboardStats
    {
        public int TotalRooms { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalBookings { get; set; }
        public double TotalRevenue { get; set; }
        public List<RevenuePoint> RevenueTrend { get; set; } = new();
    }

    public class RevenuePoint
    {
        public string Label { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
