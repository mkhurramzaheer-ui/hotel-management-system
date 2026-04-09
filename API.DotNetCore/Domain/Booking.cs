using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Booking
    {
        public int Id { get; set; }
        // Foreign keys
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Room Room { get; set; } = null!;
        public Billing? Billing { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
