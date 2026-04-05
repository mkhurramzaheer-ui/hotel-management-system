using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Billing
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Unpaid";
        public DateTime BillingDate { get; set; } = DateTime.UtcNow;
    }
}
