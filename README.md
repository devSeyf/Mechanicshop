
```markdown
# MechanicShop — Automotive Service Management System

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![Blazor](https://img.shields.io/badge/Frontend-Blazor%20WASM-512BD4?style=for-the-badge&logo=blazor)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2B%20CQRS-blue?style=for-the-badge)
![Testing](https://img.shields.io/badge/Testing-Testcontainers%20%2B%20xUnit-brightgreen?style=for-the-badge)

A full-stack, multi-role automotive service management platform built with **ASP.NET Core Minimal APIs** and a **Blazor WebAssembly** front end. MechanicShop enables workshops to manage the complete service lifecycle — from customer intake and scheduling to repair execution, billing, and real-time shop-floor updates — powered by Clean Architecture, CQRS, and an extensive automated test suite.

---

## 💡 Overview

MechanicShop supports two distinct operational roles:
* 👔 **Manager** — Oversees scheduling, customers, employees, repair/parts catalog, billing, and shop-wide dashboards.
* 🔧 **Labor** — Executes assigned work orders, updates repair progress, and views real-time shop-floor updates.

> **Domain Focus:** The system manages physical service spots, appointment durations, and working hours. Core domain logic handles complex spot allocation, overdue booking cleanups, and cancellation thresholds rather than simple CRUD operations.

---

## ✨ Key Features

- 📋 **Work Order Lifecycle** — End-to-end tracking of repair jobs with live updates delivered via **SignalR**.
- 📅 **Smart Scheduling** — Spot-based scheduling enforcing shop capacity, duration constraints, and automated cleanup of expired bookings.
- 👥 **Customers & Vehicles** — Centralized management of customer profiles and vehicle histories.
- 📦 **Tasks & Parts Catalog** — Standardized repair tasks, labor durations, and inventory pricing for accurate estimation.
- 🧾 **Billing & Invoicing** — Itemized invoice generation with dynamic PDF export powered by **QuestPDF**.
- 📊 **Executive Dashboard** — Key metrics for managers including operational load, active work orders, and revenue summaries.
- 🔒 **Role-Based Security** — ASP.NET Core Identity with **JWT Bearer** authentication and strict `ManagerOnly` authorization policies.
- ⚡ **Hybrid Caching** — Integrated local and distributed caching (`Microsoft.Extensions.Caching.Hybrid`) optimizing read-heavy workloads.
- 📈 **Full Observability** — Structured logging via **Serilog → Seq**, plus distributed tracing and metrics via **OpenTelemetry → Prometheus**.
- 📚 **Interactive API Docs** — Dual API documentation using **OpenAPI/Swagger** and **Scalar**.
- 🧪 **Comprehensive Testing** — Multi-tiered test suite (Unit, Subcutaneous, and Integration) using **Testcontainers** with real SQL Server instances.

---

## 🛠️ Tech Stack

| Layer | Technologies |
| :--- | :--- |
| **Backend Framework** | .NET 10, ASP.NET Core Minimal APIs |
| **Frontend** | Blazor WebAssembly |
| **Architecture** | Clean Architecture, CQRS (MediatR) |
| **Persistence & Auth** | Entity Framework Core, SQL Server, ASP.NET Core Identity (JWT) |
| **Real-Time & Docs** | SignalR, QuestPDF (PDF Invoicing) |
| **Caching** | Hybrid Cache (`Microsoft.Extensions.Caching.Hybrid`) |
| **Validation** | FluentValidation |
| **Observability** | Serilog, Seq, OpenTelemetry, Prometheus |
| **API Documentation** | Swashbuckle (Swagger), Scalar, API Versioning |
| **Testing** | xUnit, NSubstitute, Testcontainers (SQL Server), `Microsoft.AspNetCore.Mvc.Testing` |
| **Infrastructure** | Docker, Docker Compose |

---

## 🏛️ Architecture & Project Structure

The codebase strictly adheres to **Clean Architecture** principles, maintaining unidirectional dependency flow towards the Domain core:

```text
                               ┌─────────────────────────┐
                               │    Mechanicshop.Client  │ (Blazor WASM)
                               └────────────┬────────────┘
                                            │
                               ┌────────────▼────────────┐
                               │     Mechanicshop.Api    │ (Minimal APIs & Host)
                               └──────┬───────────┬──────┘
                                      │           │
           ┌──────────────────────────▼┐         ┌▼──────────────────────────┐
           │ Mechanicshop.Infrastructure│         │   Mechanicshop.Contracts  │
           └──────────────┬────────────┘         └──────────────┬────────────┘
                          │                                     │
                          └─────────────┐         ┌─────────────┘
                                       ┌▼─────────▼─────────────┐
                                       │Mechanicshop.Application│ (CQRS Commands/Queries)
                                       └──────────┬─────────────┘
                                                  │
                                       ┌──────────▼─────────────┐
                                       │   Mechanicshop.Domain  │ (Entities & Domain Logic)
                                       └────────────────────────┘

```

### Feature-Based Application Slices

Inside `Mechanicshop.Application`, logic is organized by feature rather than layer type:
`Billing` • `Customers` • `Dashboard` • `Identity` • `Labors` • `RepairTasks` • `Scheduling` • `WorkOrders`

---

## 🧪 Automated Testing Strategy

Testing is divided into four targeted projects to ensure reliability at every layer:

* **`Domain.UnitTests`** — Tests pure domain logic, entity states, and business rules in complete isolation.
* **`Application.UnitTests`** — Validates command and query handlers using mocked dependencies (**NSubstitute**).
* **`Application.SubcutaneousTests`** — Exercises application pipelines directly through Dependency Injection without HTTP overhead.
* **`Api.IntegrationTests`** — End-to-end HTTP tests running against disposable, real SQL Server instances via **Testcontainers**.

Run the full test suite with:

```bash
dotnet test

```

---

## 🚀 Getting Started

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/sql-server/) (Local DB or Docker)
* [Docker Desktop](https://www.docker.com/) *(Optional: required for running Seq and Prometheus)*

### Quick Start (Local Run)

1. **Clone the repository:**
```bash
git clone [https://github.com/AbdAlAleem-Hassan/Mechanicshop.git](https://github.com/AbdAlAleem-Hassan/Mechanicshop.git)
cd Mechanicshop

```


2. **Restore and Run:**
```bash
dotnet restore
dotnet run --project src/Mechanicshop.Api

```


> *Note: The API project hosts and serves the Blazor WebAssembly client directly.*


3. **(Optional) Launch Observability Infrastructure:**
```bash
docker compose -f containers/seq/docker-compose.yml up -d
docker compose -f containers/prometheus/docker-compose.yml up -d

```



---

## 🔍 Exploring the API

* **Interactive Documentation:** Once running, navigate to `/swagger` or `/scalar` in your browser to inspect and execute endpoints.
* **HTTP Client Files:** A pre-configured `Requests/requests.http` file is included for instant endpoint testing directly within VS Code or JetBrains Rider.

---

## 👤 Author & Architecture Notes

Designed and implemented end-to-end as a production-grade .NET solution. Key architectural highlights include:

* Application of **CQRS** and **Clean Architecture** patterns tailored to complex scheduling and billing domains.
* Dual-role UI implementation using **Blazor WebAssembly** with real-time **SignalR** synchronization.
* Production-ready observability integration (structured logging, tracing, and metrics) alongside containerized integration testing.

```

```
