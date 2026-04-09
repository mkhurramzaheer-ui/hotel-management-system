import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api';

export interface RoomDto {
  id: number;
  roomNumber: string;
  type: string;
  pricePerNight: number;
  isAvailable: boolean;
}

export interface CustomerDto {
  id: number;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  fullName?: string | null;
}

export interface BookingDto {
  id: number;
  customerId: number;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  totalAmount: number;
  status?: string | null;
  createdAt?: string;
  customer?: CustomerDto;
  room?: RoomDto;
}

export interface CreateBookingRequest {
  customerId: number;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  totalAmount: number;
  status: string;
  createdAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class HotelService {
  private readonly http = inject(HttpClient);
  private readonly api = inject(ApiService);

  getRooms(): Observable<RoomDto[]> {
    return this.http.get<RoomDto[]>(this.api.createUrl(this.api.endpoints.rooms));
  }

  getBookings(): Observable<BookingDto[]> {
    return this.http.get<BookingDto[]>(
      this.api.createUrl(this.api.endpoints.bookings),
    );
  }

  createBooking(payload: CreateBookingRequest): Observable<BookingDto> {
    return this.http.post<BookingDto>(
      this.api.createUrl(this.api.endpoints.bookings),
      payload,
    );
  }
}
