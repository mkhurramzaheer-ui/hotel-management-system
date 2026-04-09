import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  readonly baseUrl = '';
  readonly endpoints = {
    login: '/api/Auth/login',
    customers: '/api/Customers',
    rooms: '/api/Rooms',
    bookings: '/api/Bookings',
  };

  createUrl(path: string): string {
    return `${this.baseUrl}${path}`;
  }
}
