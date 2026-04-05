using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class BookingValidator : AbstractValidator<BookingDto>
    {
        public BookingValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0);
            RuleFor(x => x.RoomId).GreaterThan(0);
            RuleFor(x => x.TotalAmount).GreaterThan(0);
            RuleFor(x => x.CheckOutDate)
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("Check-out date must be after check-in date.");
        }
    }
}
