
```markdown
# Ride Lifecycle Management Module

---

## 📋 Overview

This module manages the complete lifecycle of a ride request—from initiation, acceptance, navigation, completion, to feedback and dispute resolution. It enables seamless coordination between passengers, drivers, and admins through REST APIs and real-time SignalR communication.

---

## 🚀 Features

- Passenger ride requests (instant and scheduled)
- Driver ride acceptance
- Real-time ride status and location updates via SignalR (`RideHub`)
- Fare calculation based on distance, time, and surge pricing
- Ride completion and cancellation handling
- Feedback and rating system for passengers and drivers
- Admin oversight for dispute resolution and ride history

---

## 🗂️ Project Folder Structure

```

RideApp/
│
├── Controllers/
│   └── RideController.cs
│   └── FeedbackController.cs
│
├── Models/
│   └── Ride.cs
│   └── Feedback.cs
│   └── AcceptRideDto.cs
│
├── Services/
│   └── FareCalculator.cs
│   └── DriverAssigner.cs
│
├── Hubs/
│   └── RideHub.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Program.cs
├── Startup.cs (if using .NET 5 or earlier)
└── appsettings.json

````

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (or the version compatible with your environment)
- Visual Studio 2022 or later with ASP.NET Core workload
- SQL Server / SQLite (configured in `appsettings.json`)
- Git installed for version control

---

### Setup Instructions

1. **Clone the repo and checkout branch**

```bash
git clone https://github.com/MrGWa/Uber_Clone.git
cd Uber_Clone
git checkout Ride-Lifecycle-Management
````

2. **Restore and build**

```bash
dotnet restore
dotnet build
```

3. **Configure database connection string** in `appsettings.json`

4. **Run EF Core migrations**

```bash
dotnet ef database update
```

5. **Run the app**

```bash
dotnet run --project RideApp
```

The app will launch with Swagger UI available at:

```
https://localhost:<port>/swagger
```

---

## 🔗 API Endpoints (`RideController`)

| Endpoint             | Method | Description                               | Request Body Example                                                           |
| -------------------- | ------ | ----------------------------------------- | ------------------------------------------------------------------------------ |
| `/api/request-ride`  | POST   | Passenger requests a new ride             | `{ "pickup_location": "...", "dropoff_location": "...", "passenger_id": 123 }` |
| `/api/accept-ride`   | POST   | Driver accepts a ride request             | `{ "ride_id": 456, "driver_id": 789 }`                                         |
| `/api/complete-ride` | POST   | Mark ride as completed and calculate fare | `{ "ride_id": 456 }`                                                           |
| `/api/report-issue`  | POST   | Report an issue during or after the ride  | `{ "ride_id": 456, "description": "Issue details..." }`                        |
| `/api/feedback`      | POST   | Submit feedback and rating                | `{ "ride_id": 456, "giver_id": 123, "rating": 5, "comment": "Great ride!" }`   |

---

## 📡 Real-Time Updates with SignalR (`RideHub`)

* **Hub URL:** `/ridehub`
* Facilitates live communication between passengers and drivers:

  * Ride status updates (accepted, en route, arrived, completed)
  * Location tracking
  * Cancellation notifications

---

## 🔧 Integration Guide

* **Driver-side integration:**

  * Use `/api/accept-ride` to accept ride requests.
  * Connect to `RideHub` for real-time updates on assigned rides.

* **Passenger-side integration:**

  * Use `/api/request-ride` to request rides.
  * Subscribe to `RideHub` for live status and driver location updates.
  * Submit feedback via `/api/feedback`.

* **Admin-side integration:**

  * Access ride and feedback data for audits and dispute handling.
  * Manage ride cancellations and resolve disputes through APIs.



## 🛠️ Development Notes

* The project namespace is `RideApp`.
* Dependency Injection and services are configured in `Program.cs`.
* Logging uses ASP.NET Core’s built-in framework.
* Use the provided `.http` files or Swagger UI to test API endpoints.
* Work in feature branches and create PRs targeting `Ride-Lifecycle-Management`.

