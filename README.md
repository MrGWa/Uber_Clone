# 🚗 Uber Clone - Admin Dashboard & System Analytics Backend

This README documents the complete implementation of the **Admin Dashboard and System Analytics** module for our Uber Clone project. This backend-only module follows Clean Architecture principles and is built using C# with .NET 9, EF Core (in-memory DB), and is fully testable via Postman or terminal (curl).

---

## 📁 Project Structure (Solution Layout)

```
UberClone.Api/                → Entry-point Web API
├── Controllers/AdminController.cs
├── Program.cs

UberClone.Application/       → DTOs and Use Cases
├── DTOs/Admin/*.cs

UberClone.Infrastructure/    → Services and EF DbContext
├── Services/Admin/*.cs
├── Persistence/AppDbContext.cs

UberClone.Domain/            → Core entities
├── Entities/*.cs
```

---

## ✅ Features Implemented

### 1. Revenue Report

* Endpoint: `POST /api/admin/reports/revenue`
* Accepts a date range
* Returns total rides and revenue
* **DTOs:** `ReportRequestDto`, `RevenueReportDto`

### 2. User Activity Report

* Endpoint: `GET /api/admin/reports/user-activity`
* Returns total rides, spending, and last ride date for each user
* **DTO:** `UserActivityDto`

### 3. Support Tickets Management

* `POST /api/admin/tickets` → Create a ticket
* `GET /api/admin/tickets` → View all tickets
* `PUT /api/admin/tickets/{id}` → Resolve a ticket
* Includes `View Ticket Details` and `Resolve Dispute`

### 4. Tariff Configuration

* `POST /api/admin/tariffs` → Add/update regional tariffs
* `GET /api/admin/tariffs` → View all tariffs

### 5. Promo Code Management

* `POST /api/admin/promocodes` → Create a promo code
* `GET /api/admin/promocodes` → View all promo codes
* `PUT /api/admin/promocodes/{id}/deactivate` → Disable a promo code

### 6. Audit Log System

* Every major admin action is logged (resolving tickets, updating tariffs, etc.)
* `GET /api/admin/audit-logs` → View all logs
* **Entity:** `AuditLog`

### 7. System Health Monitoring

* Endpoint: `GET /api/admin/health`
* Verifies API responsiveness and DB availability

### 8. Real-Time Driver Location Monitoring

* `POST /api/admin/drivers/location` → Update driver GPS location
* `GET /api/admin/drivers/location` → View all active driver locations

---

## 🧪 Testing Guide (Postman or curl)

### Example: Create Support Ticket (POST)

```bash
curl -X POST http://localhost:5103/api/admin/tickets \
     -H "Content-Type: application/json" \
     -d "{\"userId\": \"{VALID-GUID}\", \"issue\": \"Driver late\"}"
```

### Example: Update Ticket (PUT)

```bash
curl -X PUT http://localhost:5103/api/admin/tickets/1 \
     -H "Content-Type: application/json" \
     -d "{\"adminResponse\":\"Refunded\",\"status\":\"Resolved\"}"
```

### View Revenue Report (POST)

```bash
curl -X POST http://localhost:5103/api/admin/reports/revenue \
     -H "Content-Type: application/json" \
     -d "{\"startDate\":\"2024-01-01\",\"endDate\":\"2024-12-31\"}"
```

### System Health Check (GET)

```bash
curl http://localhost:5103/api/admin/health
```

---

## 🧠 Notes

* In-memory DB used (EF Core) — no external DB setup required
* All endpoints are tested using Postman and optionally `curl`
* Each feature follows Clean Architecture (DTOs + Services + Entities)

---

## 🎯 Final Status

**All features from the Admin Dashboard & System Analytics use case diagram have been fully implemented and tested.**

This backend is ready for integration with any frontend or further enhancements such as authentication, exporting reports, or cloud deployment.

---

Built by: Tamar
Role: Admin Dashboard & Analytics Developer



---
# 🚕 Uber Clone – Ride Lifecycle Management Module

This README documents the complete implementation of the **Ride Lifecycle Management** module for the Uber Clone project. This backend module is developed in **C# with .NET 9**, follows **Clean Architecture**, and integrates seamlessly with the existing `Admin Dashboard & System Analytics` module.

This module ensures a full **end-to-end ride experience**, from requesting a ride to completing and rating it. It enables dynamic interactions between drivers and passengers, handles fare calculations, and supports real-time ride updates and tracking.

---

## 🗂️ Project Structure (Solution Layout)

```

UberClone.sln
├── UberClone.Api/                   → Entry-point Web API
│   └── Controllers/RideController.cs
├── UberClone.Application/          → DTOs and Interfaces
│   ├── DTOs/Ride/RideRequestDto.cs
│   ├── DTOs/Ride/RideAcceptedDto.cs
│   ├── DTOs/Ride/RideCompletedDto.cs
│   └── Interfaces/IRideService.cs
├── UberClone.Infrastructure/       → Services and EF DbContext
│   ├── Services/RideService.cs
│   └── Persistence/AppDbContext.cs
├── UberClone.Domain/               → Domain Entities
│   ├── Entities/Ride.cs
│   └── Entities/RideStatus.cs
└── UberClone.Tests/                → Unit Tests
└── RideServiceTests.cs

```


## ✅ Features Implemented

### 🚦 1. Ride Request
- **Endpoint**: `POST /api/ride/request`
- **DTO**: `RideRequestDto`
- **Function**: `RideService.RequestRide`
- **Description**: Creates a ride request with `PassengerId`, `Pickup`, and `Dropoff`. Automatically assigns status as `Pending`.

---

### 🛑 2. Ride Acceptance (by driver)
- **Endpoint**: `POST /api/ride/accept`
- **DTO**: `RideAcceptedDto`
- **Function**: `RideService.AcceptRide`
- **Description**: Updates ride with `DriverId` and sets status to `Accepted`.

---

### 🏁 3. Ride Completion
- **Endpoint**: `POST /api/ride/complete`
- **DTO**: `RideCompletedDto`
- **Function**: `RideService.CompleteRide`
- **Description**: Completes ride, sets status to `Completed`, and calculates fare based on time elapsed.

---

## 🧱 Design Patterns Used

### 🔹 Clean Architecture
The module strictly follows Clean Architecture principles:
- **Domain Layer**: Pure business models like `Ride.cs` and enums like `RideStatus.cs`
- **Application Layer**: Contracts (interfaces) and DTOs
- **Infrastructure Layer**: Data access and service logic
- **API Layer**: Exposes controller endpoints

### 🔹 Dependency Injection
Used throughout the project (see `Program.cs` in `UberClone.Api`):
```csharp
builder.Services.AddScoped<IRideService, RideService>();
````

Enables loose coupling between `RideController` and `RideService`.

### 🔹 Service Pattern

`RideService.cs` in `UberClone.Infrastructure.Services` encapsulates all ride-related business logic.

### 🔹 Repository/DbContext Pattern

Uses `AppDbContext.cs` for EF Core in-memory storage (file: `UberClone.Infrastructure.Persistence.AppDbContext.cs`).

### 🔹 Constants Enum Pattern

Used in `RideStatus.cs` to manage ride state strings and avoid hardcoding:

```
public static class RideStatus
{
    public const string Pending = "Pending";
    public const string Accepted = "Accepted";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
}
```

**Referenced In**: `RideService.cs`

---

## ⚙️ DTOs (Data Transfer Objects)

**Located in**: `UberClone.Application.DTOs.Ride/`

* `RideRequestDto.cs`: Contains `PassengerId`, `PickupLocation`, `DropoffLocation`
* `RideAcceptedDto.cs`: Contains `RideId`, `DriverId`
* `RideCompletedDto.cs`: Contains `RideId`

---

## 🧪 Unit Tests

**File**: `UberClone.Tests/RideServiceTests.cs`

Test coverage includes:

* Ride request creation
* Ride acceptance by driver
* Ride completion and fare calculation
* Invalid operations (e.g., accepting/completing non-existent rides)

Tests use `EF Core InMemory` to simulate database behavior.

---

## 🧠 Notes

* Status values are enforced through `RideStatus` class to avoid hardcoding.
* Fare calculation is currently time-based: `$5.00 base fare + $1/min`.
* `AppDbContext` is used in-memory for simplified testing and development.
* Fully compatible with Admin Dashboard & Analytics module.

---

## ✨ Future Enhancements

* Ride cancellation support
* Driver/passenger feedback system
* Scheduled rides (time-slot based)
* Contact/communication channels
* Ride history and reports

---

## 👤 Built by

**Nini Jakhaia** – Ride Lifecycle Management Developer
Based on collaborative Clean Architecture with Collaborators


