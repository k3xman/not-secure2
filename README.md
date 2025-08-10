# Todo Application

A simple Todo application built with ASP.NET Core 9.0 and Microsoft Aspire, featuring PostgreSQL database integration.

## Features

- ✅ Create new todo items with title and description
- ✅ Mark todo items as complete/incomplete
- ✅ Delete todo items
- ✅ View all todo items with creation and completion timestamps
- ✅ Responsive Bootstrap UI with Bootstrap Icons
- ✅ PostgreSQL database integration via Microsoft Aspire

## Architecture

- **AppHost1**: Microsoft Aspire Application Host that orchestrates the distributed application
- **WebApplication1**: ASP.NET Core MVC web application with Entity Framework Core
- **Database**: PostgreSQL database managed by Aspire

## Prerequisites

- .NET 9.0 SDK
- Docker (for running PostgreSQL container)

## Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd not-secure1
   ```

2. **Run the Aspire Application Host**
   ```bash
   cd AppHost1/AppHost1
   dotnet run
   ```

   This will:
   - Start a PostgreSQL database container
   - Launch the web application
   - Open the Aspire dashboard in your browser

3. **Access the Application**
   - Web Application: http://localhost:8081
   - Aspire Dashboard: http://localhost:8080

## Database Schema

The application uses a simple `TodoItems` table with the following structure:

- `Id` (int, Primary Key)
- `Title` (string, Required, Max 200 characters)
- `Description` (string, Optional, Max 1000 characters)
- `IsCompleted` (bool)
- `CreatedAt` (DateTime, UTC)
- `CompletedAt` (DateTime?, UTC, nullable)

## Development

The application automatically creates the database and tables on first run using `context.Database.EnsureCreated()`.

## Technologies Used

- **Backend**: ASP.NET Core 9.0, Entity Framework Core
- **Database**: PostgreSQL with Npgsql
- **Frontend**: Bootstrap 5, Bootstrap Icons
- **Orchestration**: Microsoft Aspire
- **Container**: Docker

## Project Structure

```
├── AppHost1/                 # Aspire Application Host
│   └── AppHost1/
│       ├── AppHost.cs       # Application orchestration
│       └── AppHost1.csproj  # Project configuration
├── WebApplication1/          # Web Application
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   └── TodoController.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   ├── ErrorViewModel.cs
│   │   └── TodoItem.cs
│   ├── Views/
│   │   ├── Shared/
│   │   │   └── _Layout.cshtml
│   │   └── Todo/
│   │       ├── Index.cshtml
│   │       └── Create.cshtml
│   └── Program.cs
└── AppHost1.sln             # Solution file
``` 