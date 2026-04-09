import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { ApiService } from './api';

interface LoginRequest {
  username: string;
  password: string;
}

interface CustomerRegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
}

type LoginResponse =
  | string
  | {
      token?: string;
      jwtToken?: string;
      accessToken?: string;
      bearerToken?: string;
    };

type LoginResponseObject = Exclude<LoginResponse, string>;

type CustomerRegistrationResponse =
  | unknown
  | {
      id?: number;
      customerId?: number;
    };

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly api = inject(ApiService);
  private readonly tokenKey = 'token';
  private readonly customerIdKey = 'customerId';

  login(payload: LoginRequest): Observable<string> {
    return this.http
      .post<LoginResponse>(this.api.createUrl(this.api.endpoints.login), payload)
      .pipe(
        map((response) => this.extractToken(response)),
        tap((token) => {
          this.setToken(token);

          const customerId = this.extractCustomerIdFromToken(token);
          if (customerId !== null) {
            this.setCustomerId(customerId);
          }
        }),
      );
  }

  registerCustomer(
    payload: CustomerRegistrationRequest,
  ): Observable<CustomerRegistrationResponse> {
    return this.http
      .post<CustomerRegistrationResponse>(
        this.api.createUrl(this.api.endpoints.customers),
        payload,
      )
      .pipe(
        tap((response) => {
          const customerId = this.extractCustomerIdFromResponse(response);
          if (customerId !== null) {
            this.setCustomerId(customerId);
          }
        }),
      );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  clearToken(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.customerIdKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getCurrentCustomerId(): number | null {
    const storedCustomerId = localStorage.getItem(this.customerIdKey);
    if (storedCustomerId) {
      const parsedCustomerId = Number(storedCustomerId);
      if (!Number.isNaN(parsedCustomerId)) {
        return parsedCustomerId;
      }
    }

    const token = this.getToken();
    if (!token) {
      return null;
    }

    const customerId = this.extractCustomerIdFromToken(token);
    if (customerId !== null) {
      this.setCustomerId(customerId);
    }

    return customerId;
  }

  private setCustomerId(customerId: number): void {
    localStorage.setItem(this.customerIdKey, String(customerId));
  }

  private extractToken(response: LoginResponse): string {
    if (typeof response === 'string' && response.trim()) {
      return response;
    }

    const responseObject = response as LoginResponseObject;
    const token =
      responseObject.token ??
      responseObject.jwtToken ??
      responseObject.accessToken ??
      responseObject.bearerToken;

    if (!token) {
      throw new Error('Token was not found in the login response.');
    }

    return token;
  }

  private extractCustomerIdFromResponse(
    response: CustomerRegistrationResponse,
  ): number | null {
    if (!response || typeof response !== 'object') {
      return null;
    }

    const candidate = response as { id?: unknown; customerId?: unknown };
    const rawValue = candidate.customerId ?? candidate.id;
    const customerId = Number(rawValue);

    return Number.isNaN(customerId) ? null : customerId;
  }

  private extractCustomerIdFromToken(token: string): number | null {
    try {
      const parts = token.split('.');
      if (parts.length < 2) {
        return null;
      }

      const payload = this.decodeBase64Url(parts[1]);
      const claims = JSON.parse(payload) as Record<string, unknown>;

      const claimKeys = [
        'customerId',
        'customerid',
        'sub',
        'nameid',
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier',
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid',
      ];

      for (const key of claimKeys) {
        const rawValue = claims[key];
        if (rawValue === undefined || rawValue === null || rawValue === '') {
          continue;
        }

        const customerId = Number(rawValue);
        if (!Number.isNaN(customerId)) {
          return customerId;
        }
      }
    } catch {
      return null;
    }

    return null;
  }

  private decodeBase64Url(value: string): string {
    const normalizedValue = value.replace(/-/g, '+').replace(/_/g, '/');
    const padding = '='.repeat((4 - (normalizedValue.length % 4)) % 4);
    return atob(normalizedValue + padding);
  }
}
