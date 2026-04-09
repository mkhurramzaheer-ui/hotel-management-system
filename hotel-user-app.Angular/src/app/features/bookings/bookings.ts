import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth';
import { BookingDto, HotelService } from '../../core/services/hotel';

@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule],
  templateUrl: './bookings.html',
  styleUrl: './bookings.scss',
})
export class MyBookingsComponent implements OnInit {
  private readonly hotelService = inject(HotelService);
  private readonly authService = inject(AuthService);

  readonly bookings = signal<BookingDto[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');
  readonly currentCustomerId = this.authService.getCurrentCustomerId();

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.hotelService.getBookings().subscribe({
      next: (bookings) => {
        this.bookings.set(this.filterBookings(bookings));
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.errorMessage.set(
          'Could not load bookings. Make sure you are logged in and the token is valid.',
        );
      },
    });
  }

  private filterBookings(bookings: BookingDto[]): BookingDto[] {
    if (!this.currentCustomerId) {
      return bookings;
    }

    return bookings.filter(
      (booking) => booking.customerId === this.currentCustomerId,
    );
  }
}
