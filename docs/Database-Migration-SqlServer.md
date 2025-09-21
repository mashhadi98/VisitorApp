# Database Migration: MySQL to SQL Server

## Overview
This document outlines the migration from MySQL to SQL Server for the VisitorApp project.

## Changes Made

### 1. Package References Updated
- **Removed**: `Pomelo.EntityFrameworkCore.MySql` (8.0.3)
- **Removed**: `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.11) 
- **Added**: `Microsoft.EntityFrameworkCore.SqlServer` (8.0.19)

### 2. Database Configuration Changes

#### DatabaseServiceExtensions.cs
- Changed from `UseMySql()` to `UseSqlServer()`
- Removed MySQL-specific server version configuration
- Updated retry resilience configuration for SQL Server
- Updated connection string validation logic

#### TestDbContextFactory.cs  
- Updated design-time factory to use SQL Server
- Removed Pomelo MySQL references
- Added SQL Server retry configuration

### 3. Connection Strings Updated

#### Production (appsettings.json)
```json
{
  "ConnectionStrings": {
    "ApplicationDb": "Server=(localdb)\\mssqllocaldb;Database=VisitorAppDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### Development (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "ApplicationDb": "Server=(localdb)\\mssqllocaldb;Database=VisitorAppDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### 4. Migration Files
- **Removed**: All MySQL-specific migration files
- **Generated**: `InitialSqlServer` migration with:
  - Identity tables (Users, Roles, etc.)
  - OpenIddict tables for OAuth 2.0
  - Application-specific tables (Products, AuditLogs)

### 5. Retry Resilience Configuration
SQL Server retry configuration includes:
- **Max Retry Count**: 5 attempts
- **Max Retry Delay**: 30 seconds
- **Command Timeout**: 60 seconds
- **Transient Error Handling**: Automatic retry on connection failures

## SQL Server Features Used

### 1. LocalDB
- Uses SQL Server LocalDB for development
- No SQL Server installation required
- Automatic database creation

### 2. Connection Features
- **Trusted_Connection**: Windows Authentication
- **MultipleActiveResultSets**: Enables MARS
- **TrustServerCertificate**: Bypasses certificate validation

### 3. Entity Framework Features
- **EnableRetryOnFailure**: Transient fault handling
- **MigrationsAssembly**: Proper assembly configuration
- **Sensitive Data Logging**: Development debugging (dev only)

## Migration Benefits

### 1. Better Integration
- Native Microsoft stack integration
- Better Visual Studio support
- Enhanced debugging capabilities

### 2. Performance
- Optimized for .NET applications
- Better connection pooling
- Improved query performance

### 3. Development Experience
- LocalDB for easy development setup
- No external database server required
- Integrated with Visual Studio

## Testing
Use the updated `Identity.http` file to test the endpoints with SQL Server backend.

## Troubleshooting

### Common Issues
1. **LocalDB not found**: Install SQL Server Express LocalDB
2. **Connection failures**: Check Windows Authentication
3. **Migration errors**: Ensure proper permissions

### Connection String Examples
```csharp
// LocalDB (Development)
"Server=(localdb)\\mssqllocaldb;Database=VisitorAppDb;Trusted_Connection=true"

// SQL Server Express
"Server=.\\SQLEXPRESS;Database=VisitorAppDb;Trusted_Connection=true"

// SQL Server with Authentication  
"Server=server-name;Database=VisitorAppDb;User Id=username;Password=password"
``` 