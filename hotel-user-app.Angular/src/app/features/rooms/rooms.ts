import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  CreateBookingRequest,
  HotelService,
  RoomDto,
} from '../../core/services/hotel';
import { AuthService } from '../../core/services/auth';

@Component({
  selector: 'app-rooms',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './rooms.html',
  styleUrl: './rooms.scss',
})
export class RoomsComponent implements OnInit {
  private readonly hotelService = inject(HotelService);
  private readonly authService = inject(AuthService);
  private readonly formBuilder = inject(FormBuilder);

  readonly rooms = signal<RoomDto[]>([]);
  readonly selectedRoom = signal<RoomDto | null>(null);
  readonly isLoading = signal(true);
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');
  readonly bookingMessage = signal('');
  readonly bookingError = signal('');
  readonly currentCustomerId = this.authService.getCurrentCustomerId();

  readonly bookingForm = this.formBuilder.nonNullable.group({
    customerId: [this.currentCustomerId ?? 0, [Validators.required, Validators.min(1)]],
    checkInDate: ['', [Validators.required]],
    checkOutDate: ['', [Validators.required]],
  });

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.hotelService.getRooms().subscribe({
      next: (rooms) => {
        this.rooms.set(rooms);
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.errorMessage.set('Could not load rooms from the API.');
      },
    });
  }

  selectRoom(room: RoomDto): void {
    this.selectedRoom.set(room);
    this.bookingMessage.set('');
    this.bookingError.set('');

    if (this.currentCustomerId) {
      this.bookingForm.controls.customerId.setValue(this.currentCustomerId);
    }
  }

  createBooking(): void {
    const selectedRoom = this.selectedRoom();

    if (!selectedRoom) {
      this.bookingError.set('Select a room first.');
      return;
    }

    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      return;
    }

    const formValue = this.bookingForm.getRawValue();
    const totalAmount = this.calculateTotalAmount(
      selectedRoom.pricePerNight,
      formValue.checkInDate,
      formValue.checkOutDate,
    );

    if (totalAmount <= 0) {
      this.bookingError.set('Check-out date must be after check-in date.');
      return;
    }

    const payload: CreateBookingRequest = {
      customerId: Number(formValue.customerId),
      roomId: selectedRoom.id,
      checkInDate: `${formValue.checkInDate}T00:00:00`,
      checkOutDate: `${formValue.checkOutDate}T00:00:00`,
      totalAmount,
      status: 'Confirmed',
      createdAt: new Date().toISOString(),
    };

    this.isSubmitting.set(true);
    this.bookingMessage.set('');
    this.bookingError.set('');

    this.hotelService.createBooking(payload).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.bookingMessage.set('Booking created successfully.');
        this.bookingForm.controls.checkInDate.reset();
        this.bookingForm.controls.checkOutDate.reset();
        this.loadRooms();
      },
      error: () => {
        this.isSubmitting.set(false);
        this.bookingError.set(
          'Booking failed. Make sure you are logged in and customer ID is correct.',
        );
      },
    });
  }

  getEstimatedTotal(): number {
    const selectedRoom = this.selectedRoom();

    if (!selectedRoom) {
      return 0;
    }

    const { checkInDate, checkOutDate } = this.bookingForm.getRawValue();
    if (!checkInDate || !checkOutDate) {
      return 0;
    }

    return this.calculateTotalAmount(
      selectedRoom.pricePerNight,
      checkInDate,
      checkOutDate,
    );
  }

  private calculateTotalAmount(
    pricePerNight: number,
    checkInDate: string,
    checkOutDate: string,
  ): number {
    const checkIn = new Date(checkInDate);
    const checkOut = new Date(checkOutDate);
    const millisecondsPerDay = 1000 * 60 * 60 * 24;
    const numberOfNights = Math.round(
      (checkOut.getTime() - checkIn.getTime()) / millisecondsPerDay,
    );

    if (numberOfNights <= 0) {
      return 0;
    }

    return numberOfNights * pricePerNight;
  }
}
