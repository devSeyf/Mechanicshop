# MechanicShop — Automotive Service Management System

A full-stack, multi-role automotive service management platform built with **ASP.NET Core Minimal APIs** and a **Blazor WebAssembly** front end. MechanicShop lets a workshop manage the entire service lifecycle — from customer intake and scheduling to repair work, billing, and real-time shop-floor updates — with Clean Architecture, CQRS, and a strong automated test suite underneath.

---

## Overview

MechanicShop is built for two roles:

- **Manager** — oversees scheduling, customers, employees, repair tasks/parts catalog, billing, and shop-wide dashboards.
- **Labor** — works assigned work orders, updates repair progress, and sees real-time shop-floor changes.

The system coordinates a limited number of physical service spots, appointment durations, and working hours, so scheduling logic (spot allocation, overdue-booking cleanup, cancellation thresholds) is a core part of the domain — not just CRUD.

---

## Features

- **Work Order Management** — create, assign, and track work orders through their full lifecycle, with live updates pushed to connected clients via **SignalR**.
- **Scheduling** — spot-based appointment scheduling within configurable shop hours, minimum appointment duration, and automatic cleanup of overdue bookings.
- **Customers & Vehicles** — manage customer profiles and their associated vehicles.
- **Repair Tasks & Parts Catalog** — maintain a catalog of repair tasks, durations, and parts used for billing and estimation.
- **Billing & Invoicing** — generate itemized invoices and export them as **PDF** documents (via QuestPDF).
- **Dashboard** — operational metrics for managers (active work orders, scheduling load, billing summaries).
- **Role-Based Access Control** — ASP.NET Core Identity with JWT authentication and a `ManagerOnly` authorization policy separating manager and labor capabilities.
- **Caching** — hybrid local + distributed caching to reduce database load on frequently read data (schedules, catalogs).
- **Observability** — structured logging with Serilog → Seq, and distributed tracing/metrics with OpenTelemetry, exported to Prometheus.
- **API Documentation** — versioned REST API with OpenAPI/Swagger and Scalar interactive docs.
- **Automated Testing** — unit, subcutaneous (in-process end-to-end), and integration tests (with Testcontainers spinning up a real SQL Server instance) covering domain logic, application handlers, and API behavior.

---

## Tech Stack

| Layer | Technologies |
|---|---|
| **Backend Framework** | .NET 10, ASP.NET Core Minimal API Endpoints |
| **Frontend** | Blazor WebAssembly |
| **Architecture** | Clean Architecture with CQRS (MediatR) |
| **Data** | Entity Framework Core, SQL Server, ASP.NET Core Identity |
| **Auth** | JWT Bearer Authentication, role-based authorization policies |
| **Validation** | FluentValidation |
| **Real-Time** | SignalR |
| **Documents** | QuestPDF (invoice generation) |
| **Caching** | Microsoft.Extensions.Caching.Hybrid |
| **Observability** | Serilog + Seq, OpenTelemetry, Prometheus |
| **API Docs** | Swashbuckle (Swagger), Scalar, API versioning |
| **Testing** | xUnit, NSubstitute, Testcontainers (MsSql), Microsoft.AspNetCore.Mvc.Testing |
| **Infrastructure** | Docker (Seq & Prometheus containers) |

---

## Architecture

The solution follows Clean Architecture, with dependencies flowing inward toward the domain:

```
Mechanicshop.Domain          → Entities, domain events, business rules (Customers, Vehicles, WorkOrder, Billing, RepairTasks, Employees)
Mechanicshop.Application       → CQRS commands/queries & handlers (MediatR), organized by feature:
                                  Billing, Customers, Dashboard, Identity, Labors, RepairTasks, Scheduling, WorkOrders
Mechanicshop.Infrastructure    → EF Core persistence, Identity, SignalR hub, caching, external concerns
Mechanicshop.Contracts        → Shared DTOs/contracts between API and Blazor client
Mechanicshop.Api              → Minimal API endpoint groups, hosts the Blazor WASM client, OpenAPI/Scalar docs
Mechanicshop.Client           → Blazor WebAssembly front end (Manager/Labor UI, SignalR client)
```

Each feature in the Application layer is self-contained (commands, queries, validators, and handlers together), which keeps business logic testable and decoupled from the API and infrastructure layers.

---

## Testing

The test suite is split into four projects to test each layer at the right level of confidence:

- **Domain.UnitTests** — pure domain logic and business rules in isolation.
- **Application.UnitTests** — command/query handlers with mocked dependencies (NSubstitute).
- **Application.SubcutaneousTests** — application layer exercised through its real dependency-injection pipeline, without going through HTTP.
- **Api.IntegrationTests** — full HTTP-level tests against the running API, backed by a real, disposable SQL Server instance via Testcontainers.

```bash
dotnet test
```

---

## Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (local or containerized)
- Docker (optional, for Seq logging and Prometheus metrics)

### Run locally

```bash
git clone https://github.com/AbdAlAleem-Hassan/Mechanicshop.git
cd Mechanicshop

dotnet restore
dotnet run --project src/Mechanicshop.Api
```

The API hosts the Blazor WebAssembly client, so running the API project serves both the backend and the front end.

### Optional: run supporting containers (logging & metrics)

```bash
docker compose -f containers/seq/docker-compose.yml up -d
docker compose -f containers/prometheus/docker-compose.yml up -d
```

### Explore the API

A ready-made `Requests/requests.http` file is included for quickly exercising the API endpoints (customers, work orders, billing, scheduling, etc.) from your editor's HTTP client. Interactive OpenAPI docs are also available via Swagger/Scalar once the API is running.

---

## My Role

I designed and built this project end-to-end as a solo full-stack exercise in production-grade .NET architecture — applying Clean Architecture and CQRS to a real-world scheduling/billing domain, building the Blazor WASM client for both manager and labor workflows, and backing it with a layered automated test suite (unit, subcutaneous, and containerized integration tests) plus observability tooling (Serilog/Seq, OpenTelemetry/Prometheus) typically seen in production systems.
