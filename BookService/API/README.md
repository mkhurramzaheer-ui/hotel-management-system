# 🏨 BookService API

A modular **Hotel Management REST API** built with **.NET 8 Web API**, **Entity Framework Core (SQLite)**, **JWT Authentication**, **Serilog Logging**, and **Docker** deployment.

This project simulates a small hotel backend that manages customers, rooms, bookings, and billing — all secured by a single JWT token.

---

## 📖 Table of Contents
1. [Purpose](#purpose)
2. [Architecture Overview](#architecture-overview)
3. [Technologies Used](#technologies-used)
4. [API Design](#api-design)
5. [Running Locally](#running-locally)
6. [Running with Docker](#running-with-docker)
7. [Authentication (JWT)](#authentication-jwt)
8. [Logging](#logging)
9. [Database](#database)
10. [Seeded Demo Data](#seeded-demo-data)
11. [Project Structure](#project-structure)
12. [Notes & Next Steps](#notes--next-steps)

---

## 🎯 Purpose

**BookService API** provides core operations for a hotel system:
- Manage **Customers**
- Manage **Rooms**
- Handle **Bookings** (Customer ↔ Room ↔ Billing)
- Manage **Billing & Payments**

It uses clean architecture, with separated domain entities, repositories, and services — all inside a single Web API project.

---

## 🧱 Architecture Overview

Client → JWT Auth → BookService.API ├── /api/customers ├── /api/rooms ├── /api/bookings └── /api/billings


- All controllers share the same API host and base URL.
- **Entity Relationships**
  - Customer → Bookings (1‑to‑many)
  - Room → Bookings (1‑to‑many)
  - Booking → Billing (1‑to‑1)
- Uses real EF Core relationships, JWT authentication, and automatic data seeding.

---

## ⚙️ Technologies Used

| Category | Technology |
|-----------|-------------|
| Framework | .NET 8 Web API |
| Language | C# 11 |
| ORM | Entity Framework Core (SQLite) |
| Authentication | JWT Bearer Tokens |
| Logging | Serilog (console + file) |
| Validation | FluentValidation |
| Database | SQLite |
| Containerization | Docker |
| Docs | Swagger / OpenAPI |

---

## 🧠 How It Works

1. **Login & Token Generation**
   - Request: `POST /api/auth/login`
   - Issues a JWT token (valid 60 minutes).
   - The same token secures all endpoints.

2. **API Operations**
   - `/api/customers`, `/api/rooms`, `/api/bookings`, `/api/billings`
   - Each endpoint performs CRUD using repositories and EF Core.

3. **Relationships**
   - A customer can have multiple bookings.
   - Each booking references a room and has one billing record.
   - EF Core tracks and enforces foreign key constraints.

4. **Logging & Validation**
   - Serilog logs requests/responses and exceptions.
   - FluentValidation checks DTOs for valid input.

---

## ▶️ Running Locally

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQLite (installed automatically with EF provider)
- Optional: Docker Desktop (for container testing)

### Commands
```bash
dotnet restore
dotnet build
dotnet run


 Running with Docker
1️⃣ Build the image
bash


docker build -t bookservice-api .
2️⃣ Run the container
bash


docker run -d -p 8080:8080 \
  -v ${PWD}/data:/app/data \
  -e ConnectionStrings__DefaultConnection="Data Source=/app/data/hotel.db" \
  --name bookservice bookservice-api
3️⃣ Inspect
bash


docker ps
docker logs bookservice
4️⃣ Access
Swagger:  
localhost
API root: 
localhost
Data (hotel.db) persists at ./data on your machine.

🔐 Authentication (JWT)
Login example:


POST /api/auth/login
Content-Type: application/json
{
  "username": "admin",
  "password": "password"
}

Logging
Serilog writes:

Console logs: visible with docker logs bookservice
File logs: /Logs/bookservice-YYYYMMDD.log
Each entry includes timestamp, log level, source, and message.

🗃 Database
SQLite file: hotel.db
Tables: Customers, Rooms, Bookings, Billings
Schema auto‑created on first run.
If you delete the file, EF Core reconstructs and reseeds it automatically.

🌱 Seeded Demo Data
Automatically generated at startup:

Table	Sample
 Customers 	 Alice Johnson, Bob Smith 
 Rooms 	 101 Deluxe, 102 Standard, 201 Suite 
 Bookings 	 Alice → Room 101 (2 nights) 
 Billings 	 Linked to the sample booking 
Use them immediately for API testing.

🧩 Project Structure


BookService.Api
├── Controllers/
│   ├── AuthController.cs
│   ├── CustomersController.cs
│   ├── RoomsController.cs
│   ├── BookingsController.cs
│   └── BillingsController.cs
├── Domain/Entities/…
├── Infrastructure/Data/BookDbContext.cs
├── Application/Services/…
├── Logs/
├── Dockerfile
└── README.md
📝 Notes & Next Steps
Environment Variables override appsettings.json keys in Docker:
ConnectionStrings__DefaultConnection
Jwt__Key, Jwt__Issuer, Jwt__Audience
Add new modules easily (e.g., Inventory or Staff) using the same pattern.
Migrate to multiple databases later without changing controller code.
Consider Docker Compose for multi‑container setups when scaling.
