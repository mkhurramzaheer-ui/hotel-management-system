using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;
        // Navigation property
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
