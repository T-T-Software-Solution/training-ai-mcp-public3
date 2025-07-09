# App.Database

[⬅ Back to Solution Overview](../../readme.md)

This project contains the Entity Framework Core database context, migrations, and seeding logic for the Dealership System solution.

## Features

- Entity Framework Core DbContext ([`AppContext`](AppContext.cs)) with DbSets for all core entities (Customer, Vehicle, Technician, ServiceAppointment, Notification, StockItem, Quotation).
- Database seeding logic in [`DbInitializer`](DbInitializer.cs) for development and demo purposes.
- SQL Server provider configuration.
- Migrations support.

## Usage

### 1. Database Seeding

The [`DbInitializer.Seed`](DbInitializer.cs) method seeds demo data if the database is empty. It is called automatically at startup by the API, MCP, and Function apps.

## Project Structure

- [`AppContext.cs`](AppContext.cs): Entity Framework Core context.
- [`DbInitializer.cs`](DbInitializer.cs): Database seeding logic.
- `Migrations/`: Entity Framework Core migrations.

## Requirements

- .NET 8.0 SDK
- SQL Server (local or remote)

## References

- [EF Core documentation](https://learn.microsoft.com/en-us/ef/core/)
- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets)