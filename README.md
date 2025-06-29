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
