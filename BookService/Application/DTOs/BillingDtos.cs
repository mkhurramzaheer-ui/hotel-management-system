using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BillingDto { public int Id { get; set; } public int BookingId { get; set; } public decimal Amount { get; set; } public string PaymentStatus { get; set; } = "Unpaid"; public DateTime BillingDate { get; set; } public BookingDto? Booking { get; set; } }

}
