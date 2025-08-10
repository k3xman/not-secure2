# Todo Application (Security Demo)

A simple Todo application built with ASP.NET Core 9.0 and Microsoft Aspire, featuring PostgreSQL database integration. **This application intentionally demonstrates bad security practices for educational purposes.**

## ⚠️ SECURITY WARNING

**This application contains intentionally bad security practices for educational demonstration only. DO NOT use this code in production applications!**

## Features

### ✅ Todo Management
- Create new todo items with title and description
- Mark todo items as complete/incomplete
- Delete todo items
- View all todo items with creation and completion timestamps
- Responsive Bootstrap UI with Bootstrap Icons
- PostgreSQL database integration via Microsoft Aspire

### 🚨 Vulnerable User Management (Educational Demo)
- **SQL Injection vulnerabilities** in all database operations
- **Poor password hashing** using MD5 (cryptographically broken)
- **Exposed password hashes** in user interface
- **No input validation** or sanitization
- **Poor session management**
- **Internal error exposure** to users
- **No access control** or authorization

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

### TodoItems Table
- `Id` (int, Primary Key)
- `Title` (string, Required, Max 200 characters)
- `Description` (string, Optional, Max 1000 characters)
- `IsCompleted` (bool)
- `CreatedAt` (DateTime, UTC)
- `CompletedAt` (DateTime?, UTC, nullable)

### Users Table (Vulnerable)
- `Id` (int, Primary Key)
- `Username` (string, Required, Max 50 characters)
- `Password` (string, Required, Max 100 characters) - **MD5 hash (BAD!)**
- `Email` (string, Required, Max 100 characters)
- `CreatedAt` (DateTime, UTC)
- `IsActive` (bool)

## 🚨 Bad Security Practices Demonstrated

### 1. SQL Injection Vulnerabilities
- **Location**: `BadSecurityService.cs`
- **Vulnerable Methods**: `GetUserByUsername`, `SearchUsers`, `CreateUser`, `DeleteUser`
- **Example Attack**: `' OR '1'='1` in username field
- **Impact**: Unauthorized access, data manipulation, potential data loss

### 2. Poor Password Security
- **Algorithm**: MD5 (cryptographically broken)
- **Salt**: None used
- **Storage**: Plain text hashes exposed in UI
- **Vulnerability**: Rainbow table attacks, hash cracking

### 3. Input Validation Issues
- **No sanitization** of user inputs
- **No validation** of data types or formats
- **Direct concatenation** of user input into SQL queries

### 4. Error Handling Problems
- **Internal errors exposed** to users
- **Stack traces** potentially revealed
- **Database structure information** leaked

### 5. Session Management Issues
- **Poor session configuration**
- **No proper logout** functionality
- **Session fixation** vulnerabilities

## Testing the Vulnerabilities

### SQL Injection Tests
1. **Bypass Authentication**: Use `' OR '1'='1` as username
2. **Drop Table**: Use `'; DROP TABLE Users; --` as username
3. **Union Attack**: Use `' UNION SELECT 1,2,3,4,5,6 --` as username

### Password Security Tests
1. **View Password Hashes**: Check user details page
2. **Crack MD5**: Use online MD5 crackers with exposed hashes
3. **Rainbow Table**: Look up common password hashes

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
│   │   ├── TodoController.cs
│   │   └── UserController.cs # VULNERABLE - Bad security practices
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Models/
│   │   ├── ErrorViewModel.cs
│   │   ├── TodoItem.cs
│   │   └── User.cs          # User model with exposed password
│   ├── Services/
│   │   └── BadSecurityService.cs # VULNERABLE - SQL injection demo
│   ├── Views/
│   │   ├── Shared/
│   │   │   └── _Layout.cshtml
│   │   ├── Todo/
│   │   │   ├── Index.cshtml
│   │   │   └── Create.cshtml
│   │   └── User/            # VULNERABLE - Exposed password views
│   │       ├── Index.cshtml
│   │       ├── Create.cshtml
│   │       ├── Login.cshtml
│   │       └── Details.cshtml
│   └── Program.cs
└── AppHost1.sln             # Solution file
```

## Educational Purpose

This application is designed to demonstrate common security vulnerabilities in web applications. It serves as a learning tool for:

- Understanding SQL injection attacks
- Recognizing poor password security practices
- Learning about input validation importance
- Understanding proper error handling
- Learning secure session management

**Remember: Never use these techniques in production applications!** 