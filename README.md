# Hotel Booking App — Full-Stack Portfolio

A full-stack Hotel Booking application built to demonstrate hands-on experience across multiple technology stacks. A single **.NET Core REST API** powers multiple frontends — an Angular user-facing app, an ASP.NET MVC admin panel, and a .NET MAUI cross-platform mobile app.

---

## Repository Structure

```
├── API.DotNetCore/            # Backend REST API
├── HotelAdmin.Mvc/            # Admin Panel (ASP.NET MVC)
├── HotelMobileApp.NetMAUI/    # Mobile App (iOS & Android)
└── hotel-user-app.Angular/    # User Booking App (Angular)
```

---

## Projects

### 1. REST API — `API.DotNetCore`
**Technologies:** .NET Core, C#, Entity Framework Core, SQL Server, Swagger / OpenAPI, xUnit (Unit Testing), Docker, Docker Compose, GitHub Actions

- RESTful API serving all frontend clients
- Containerized with Docker
- Unit tested with xUnit

---

### 2. Admin Panel — `HotelAdmin.Mvc`
**Technologies:** ASP.NET Core MVC, C#, Razor Views, Bootstrap, HttpClient

- Admin interface for managing hotels, rooms, and bookings
- Consumes the .NET Core REST API

---

### 3. Mobile App — `HotelMobileApp.NetMAUI`
**Technologies:** .NET MAUI, C#, XAML, HttpClient, MVVM Pattern, Cross-platform (iOS & Android)

- Cross-platform mobile app for hotel browsing and booking
- Consumes the .NET Core REST API

---

### 4. User Booking App — `hotel-user-app.Angular`
**Technologies:** Angular, TypeScript, RxJS, Angular HttpClient, Angular Material / Bootstrap

- Customer-facing booking interface
- Communicates with the .NET Core REST API

---

## Infrastructure & DevOps

- **Containerization:** Docker, Docker Compose
- **Source Control:** Git, GitHub
- **Testing:** xUnit, Unit Testing

---

## Getting Started

Clone the repository:

```bash
git clone https://github.com/mkhurramzaheer-ui/your-repo.git
```

Each folder has its own setup. Start the API first, then run any of the frontends.

---

## Purpose

This portfolio project demonstrates real-world full-stack development using a shared backend API with multiple frontend clients — covering REST APIs, MVC admin panels, modern SPA development with Angular, and cross-platform mobile development with .NET MAUI.
