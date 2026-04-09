# Hotel User App Angular

Frontend client for a hotel booking system built with Angular and connected to a .NET Core Web API running in Docker.

This project is part of a multi-stack portfolio setup where the same backend API can be consumed by:

- Angular
- .NET MAUI
- Blazor
- ASP.NET MVC

The goal of this repository is to demonstrate how the same hotel management backend can be used from different frontend stacks.

## Features

- Customer login using JWT returned by the .NET API
- Customer registration using the customer endpoint
- Protected navigation for authenticated users
- Rooms page connected to the live API
- Booking creation flow connected to the live API
- My Bookings page connected to the protected bookings endpoint
- Logout support

## Technology Stack

- Angular 21
- TypeScript
- Angular Router
- Angular Reactive Forms
- Angular HttpClient
- Route Guards for page protection
- HTTP Interceptor for JWT token attachment
- Bootstrap 5
- RxJS
- .NET Core API
- Docker

## Backend API

This Angular app is designed to work with the API running at:

```text
http://localhost:8080
```

Important endpoints currently used by the Angular app:

- `POST /api/Auth/login`
- `POST /api/Customers`
- `GET /api/Rooms`
- `GET /api/Bookings`
- `POST /api/Bookings`

## Security and Page Protection

This project uses JWT-based authentication.

### How login works

1. User logs in from the Angular login page.
2. Angular sends credentials to `POST /api/Auth/login`.
3. The API returns a JWT token.
4. Angular stores the token in `localStorage`.
5. All later protected requests automatically include:

```text
Authorization: Bearer <token>
```

### How pages are secured

Protected pages:

- `/rooms`
- `/my-bookings`

Public pages:

- `/login`
- `/register`

Security behavior:

- If user is not logged in, protected routes redirect to `/login`
- If user is already logged in, visiting `/login` or `/register` redirects to `/rooms`
- Navigation menu changes based on login state
- `Rooms`, `Bookings`, and `Logout` are only visible after login

Files responsible for security:

- [auth.guard.ts](D:\Hotel Managment System\hotel-user-app.Angular\src\app\core\guards\auth.guard.ts)
- [auth-interceptor.ts](D:\Hotel Managment System\hotel-user-app.Angular\src\app\core\interceptors\auth-interceptor.ts)
- [auth.ts](D:\Hotel Managment System\hotel-user-app.Angular\src\app\core\services\auth.ts)

## Local Development Setup

### 1. Start the backend API

Make sure your .NET Core API container is running and accessible on:

```text
http://localhost:8080
```

### 2. Install dependencies

```bash
npm install
```

### 3. Start Angular

```bash
npm start
```

Then open:

```text
http://localhost:4200
```

## Proxy and CORS

During local development, Angular uses a proxy so browser requests to `/api/...` are forwarded to the Docker API on port `8080`.

Proxy file:

- [proxy.conf.json](D:\Hotel Managment System\hotel-user-app.Angular\proxy.conf.json)

Why this is important:

- avoids browser CORS issues during Angular local development
- keeps frontend API calls simple by using relative `/api/...` URLs

## Application Flow

### Login

- User enters username and password
- Angular sends request to `/api/Auth/login`
- JWT token is saved in `localStorage`
- User is redirected to `/rooms`

### Register

- User enters first name, last name, email, and phone number
- Angular sends the data to `/api/Customers`

Note:

- the current backend Swagger shows customer creation but does not show a dedicated auth registration endpoint with password creation
- because of that, registration currently creates a customer record, but login credential creation depends on backend behavior

### Rooms

- Rooms are loaded from `GET /api/Rooms`
- User can select an available room
- User can submit booking details

### Bookings

- Bookings are loaded from `GET /api/Bookings`
- This endpoint requires JWT authentication
- Angular attempts to identify the logged-in customer from the JWT claims

## Current Important Notes

- The app expects the API to be available on `localhost:8080`
- If Angular was started before proxy changes, restart the dev server
- If bookings fail, verify the JWT token is valid and the backend accepts the posted booking payload
- If the JWT does not include a customer ID claim, the booking form allows manual customer ID entry
- My Bookings filters by customer ID only when a usable customer ID can be read from the token

## Scripts

```bash
npm start
npm run build
npm test
```

## Build Status

The Angular project builds successfully with:

```bash
npm run build
```

There is currently a bundle size warning, but the app compiles and runs correctly.

## Interview Value

This project shows practical experience in:

- consuming a real backend API from Angular
- JWT authentication in frontend applications
- route protection and role-style navigation behavior
- form handling with Reactive Forms
- API integration using HttpClient
- working with Docker-hosted backend services
- building the same business domain across multiple frontend stacks

## Future Improvements

- Add dedicated registration endpoint with password support
- Decode and display logged-in user information in the UI
- Add refresh token flow
- Add booking cancellation/update features
- Add better error messages from backend responses
- Add unit and integration tests for auth and booking flows
