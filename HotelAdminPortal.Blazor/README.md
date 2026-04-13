# Hotel Admin Portal — Blazor Server .NET 8

A full-featured hotel management admin portal built with Blazor Server, MudBlazor,
JWT authentication and the Mediator pattern.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | Blazor Server (.NET 8) |
| UI Library | MudBlazor v7 |
| Architecture | Mediator Pattern (custom, no MediatR dependency) |
| Auth | JWT Bearer Token (stored in-memory via AuthStateService) |
| HTTP | Typed HttpClient with auto Bearer injection |

---

## Project Structure

```
HotelAdmin/
├── Components/
│   ├── App.razor                   ← Root HTML shell
│   ├── Routes.razor                ← Router with NotFound handler
│   ├── _Imports.razor              ← Global @using directives
│   ├── Layout/
│   │   └── MainLayout.razor        ← Sidebar + AppBar + auth guard
│   ├── Pages/
│   │   ├── Login.razor             ← Two-panel login with JWT
│   │   ├── Dashboard.razor         ← Stat cards + revenue chart
│   │   ├── Rooms.razor             ← Room list with search & sort
│   │   ├── RoomDialog.razor        ← Add/Edit room popup
│   │   ├── Customers.razor         ← Customer directory
│   │   ├── CustomerDialog.razor    ← Add/Edit customer popup
│   │   ├── Bookings.razor          ← Booking records
│   │   ├── BookingDialog.razor     ← New booking popup
│   │   ├── Billing.razor           ← Billing with summary cards
│   │   ├── BillingDialog.razor     ← Add billing popup
│   │   ├── Profile.razor           ← Profile & system info
│   │   ├── NotFoundPage.razor      ← 404 page
│   │   └── Error.razor             ← Error boundary page
│   └── Shared/
│       ├── AuthGuard.razor         ← Reusable auth redirect wrapper
│       ├── ConfirmDialog.razor     ← Reusable confirm dialog
│       └── StatCard.razor          ← Reusable stat card
├── Models/
│   └── Models.cs                   ← All DTOs & request models
├── Services/
│   ├── AuthStateService.cs         ← JWT token state (scoped)
│   └── ApiService.cs               ← Mediator + all handlers
├── Properties/
│   └── launchSettings.json
├── wwwroot/
│   └── app.css                     ← All custom styles
├── HotelAdmin.csproj
├── Program.cs                      ← DI registrations
├── appsettings.json
└── appsettings.Development.json
```

---

## Setup

### 1. Prerequisites
- .NET 8 SDK
- Your Hotel API running (provides the endpoints defined in the OpenAPI spec)

### 2. Install

```bash
# Clone or copy the project files
cd HotelAdmin

# Restore packages (MudBlazor will be downloaded)
dotnet restore
```

### 3. Configure API URL

Edit `appsettings.json`:
```json
{
  "ApiBaseUrl": "https://your-api-host:port/"
}
```

For local development, edit `appsettings.Development.json`:
```json
{
  "ApiBaseUrl": "http://localhost:8080/"
}
```

### 4. Run

```bash
dotnet run
# Open http://localhost:8080
```

---

## Features

### Authentication
- JWT login via `POST /api/Auth/login`
- Token stored in `AuthStateService` (scoped, in-memory)
- Every API call automatically includes `Authorization: Bearer <token>`
- Unauthenticated users are redirected to `/login`
- Logout clears token and redirects

### Dashboard (`/`)
- Total rooms, customers, bookings, revenue stat cards
- Monthly revenue bar chart (built from billing dates)
- Recent bookings table (last 5)
- Quick action links

### Rooms (`/rooms`)
- Full CRUD: list, add, edit, delete
- Search by room number or type
- Sortable columns
- Type badges (Standard, Deluxe, Suite, Penthouse)
- Availability status indicator
- Add/Edit via popup dialog

### Customers (`/customers`)
- Full CRUD: list, add, edit, delete
- Live search by name, email, or phone
- Avatar with initials
- Add/Edit via popup dialog

### Bookings (`/bookings`)
- List all bookings with customer and room details
- Status summary chips (Confirmed / Pending / Cancelled counts)
- Create new booking via dialog (selects from existing customers & rooms)
- Delete booking with confirmation
- Color-coded status badges

### Billing (`/billing`)
- List all billing records
- Summary bar: total collected, outstanding, total records
- Filter by payment status
- Add billing linked to a booking
- Color-coded status: Paid (green) / Unpaid (red) / Partial (amber)

### Profile (`/profile`)
- Current user info
- System information panel
- Live stats summary

---

## Mediator Pattern

All API calls go through a typed mediator — no direct HttpClient usage in pages:

```csharp
// In a page:
var rooms = await Mediator.SendAsync(new GetRoomsRequest());
var ok    = await Mediator.SendAsync(new CreateRoomRequest(room));
var ok    = await Mediator.SendAsync(new UpdateRoomRequest(id, room));
var ok    = await Mediator.SendAsync(new DeleteRoomRequest(id));
```

### Adding a new operation

1. Define a request record in `ApiService.cs`:
```csharp
public record GetRoomByIdRequest(int Id) : IRequest<Room?>;
```

2. Implement its handler:
```csharp
public class GetRoomByIdHandler : IRequestHandler<GetRoomByIdRequest, Room?>
{
    private readonly ApiHttpClient _api;
    public GetRoomByIdHandler(ApiHttpClient api) => _api = api;
    public Task<Room?> HandleAsync(GetRoomByIdRequest r)
        => _api.GetAsync<Room>($"api/Rooms/{r.Id}");
}
```

3. Register in `Program.cs`:
```csharp
builder.Services.AddScoped<
    IRequestHandler<GetRoomByIdRequest, Room?>,
    GetRoomByIdHandler>();
```

4. Use anywhere in the app:
```csharp
var room = await Mediator.SendAsync(new GetRoomByIdRequest(42));
```

---

## JWT Token Flow

```
User enters credentials
        ↓
LoginHandler → POST /api/Auth/login
        ↓
Response JSON parsed for token
        ↓
AuthStateService.SetToken(token, username)
        ↓
ApiHttpClient.SetAuth() appends
"Authorization: Bearer <token>"
to every subsequent request
        ↓
Logout → AuthStateService.Logout()
→ token cleared → redirect to /login
```

> **Note:** The `LoginHandler` in `ApiService.cs` tries to deserialize the response
> as `LoginResponse` (with a `Token` property). If your API returns a different shape,
> update the deserialization logic in `LoginHandler.HandleAsync()`.

---

## Customization

### Change API base URL at runtime
```json
// appsettings.json
{ "ApiBaseUrl": "https://api.yourdomain.com/" }
```

### Change theme colors
Edit the `_theme` object in `MainLayout.razor`:
```csharp
Primary = "#your-color",
Secondary = "#your-accent",
```

### Add a new page
1. Create `Components/Pages/MyPage.razor` with `@page "/my-page"`
2. Add `@rendermode InteractiveServer`
3. Add auth check: `if (!Auth.IsAuthenticated) { Nav.NavigateTo("/login"); return; }`
4. Add nav link in `MainLayout.razor`
