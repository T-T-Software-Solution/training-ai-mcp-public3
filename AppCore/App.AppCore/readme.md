# App.AppCore

[⬅ Back to Solution Overview](../../readme.md)

This is a .NET 8.0 class library for core application logic, models, and interfaces for a service management system.  
It provides abstractions and data models for customers, vehicles, service appointments, quotations, notifications, and stock management.

## Project Structure

```
App.AppCore/
│
├── Applications/
│   └── HashUtility.cs
│
├── Interfaces/
│   ├── ICustomerService.cs
│   ├── INotificationService.cs
│   ├── IQuotationService.cs
│   ├── IServiceAppointmentService.cs
│   ├── IStockService.cs
│   └── IVehicleService.cs
│
├── Models/
│   ├── Customer.cs
│   ├── Notification.cs
│   ├── Quotation.cs
│   ├── ServiceAppointment.cs
│   ├── StockItem.cs
│   ├── Technician.cs
│   ├── Vehicle.cs
│   └── Dtos/
│       ├── CustomerDto.cs
│       ├── ServiceAppointmentDto.cs
│       └── VehicleDto.cs
│
└── ToolInformations/
    ├── CommonToolInformation.cs
    └── CustomerToolInformation.cs
```

## Key Components

- **Applications/**: Utility classes and application logic.
- **Interfaces/**: Service interfaces for dependency injection and abstraction.
- **Models/**: Entity and DTO classes representing core business data.
- **ToolInformations/**: Common constants and metadata for tools and documentation.

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Build

From the `AppCore/App.AppCore` directory, run:

```
dotnet build
```

## Usage

Reference this library in your main application or API project to access core models and service interfaces.

