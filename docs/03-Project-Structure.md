# ساختار پروژه (Project Structure)

## نمای کلی

این سند ساختار کامل پروژه و نقش هر پوشه و فایل را شرح می‌دهد. درک این ساختار برای توسعه‌دهندگان جدید و حفظ سازماندهی پروژه ضروری است.

## ساختار کلی Solution

```
VisitorApp/
├── VisitorApp.API/              # لایه ارائه (Presentation Layer)
├── VisitorApp.Application/      # لایه کاربرد (Application Layer)  
├── VisitorApp.Domain/           # لایه دامین (Domain Layer)
├── VisitorApp.Infrastructure/   # لایه زیرساخت (Infrastructure Layer)
├── VisitorApp.Persistence/      # لایه پایداری (Persistence Layer)
├── Shared/                       # کامپوننت‌های مشترک
├── docker-compose.yml            # Docker configuration
├── docker-compose.override.yml   # Docker overrides
└── VisitorApp.sln             # Solution file
```

## لایه API (VisitorApp.API)

### نمای کلی
لایه ارائه که مسئول HTTP endpoints، middleware ها و تنظیمات اپلیکیشن است.

```
VisitorApp.API/
├── Common/                       # کامپوننت‌های مشترک API
│   ├── AutoMapperConfiguration/  # تنظیمات AutoMapper
│   │   ├── AutoEntityProfile.cs     # Profile برای Entity ها
│   │   ├── AutoRequestProfile.cs    # Profile برای Request ها
│   │   └── AutoResponseProfile.cs   # Profile برای Response ها
│   │
│   ├── Configuration/           # Service Extensions
│   │   ├── ApiDocumentationServiceExtensions.cs  # Swagger config
│   │   ├── ApplicationServiceExtensions.cs       # MediatR config
│   │   ├── CorsServiceExtensions.cs             # CORS config
│   │   ├── DatabaseServiceExtensions.cs         # Database config
│   │   ├── DependencyInjectionServiceExtensions.cs # DI config
│   │   ├── LoggingServiceExtensions.cs          # Serilog config
│   │   ├── MappingServiceExtensions.cs          # AutoMapper config
│   │   ├── DatabaseServiceExtensions.cs         # SQL Server config
│   │   └── PipelineConfigurationExtensions.cs   # Middleware pipeline
│   │
│   ├── Endpoints/               # Base Classes for FastEndpoints
│   │   ├── ApiTypes.cs            # Enum for HTTP methods
│   │   ├── EndpointBase.cs        # Base endpoint classes
│   │   ├── GetEndpoint.cs         # GET endpoint base
│   │   ├── PostEndpoint.cs        # POST endpoint base
│   │   ├── PutEndpoint.cs         # PUT endpoint base
│   │   ├── DeleteEndpoint.cs      # DELETE endpoint base
│   │   ├── PatchEndpoint.cs       # PATCH endpoint base
│   │   ├── HeadEndpoint.cs        # HEAD endpoint base
│   │   ├── OptionsEndpoint.cs     # OPTIONS endpoint base
│   │   ├── TraceEndpoint.cs       # TRACE endpoint base
│   │   ├── ConnectEndpoint.cs     # CONNECT endpoint base
│   │   ├── DropdownEndpoint.cs    # Dropdown endpoints
│   │   └── PaginatedEndpoint.cs   # Paginated endpoints
│   │
│   ├── Middleware/              # Custom middleware (empty)
│   ├── Middlewares/             # Active middleware
│   │   └── LogUserInfoMiddleware.cs  # User auditing middleware
│   │
│   ├── Models/                  # API Models
│   │   ├── RequestBase.cs         # Base request class
│   │   └── SuccessResponse.cs     # Standard success response
│   │
│   └── Services/                # API Services
│       └── CurrentUserService.cs   # Current user context service
│
├── Features/                    # Feature-based organization
│   ├── Catalog/                 # Catalog domain features
│   │   ├── MappingProfile.cs      # AutoMapper profile for Catalog
│   │   └── Products/            # Product-related endpoints
│   │       ├── Create/            # Product creation endpoints
│   │       ├── Update/            # Product update endpoints
│   │       ├── Delete/            # Product deletion endpoints
│   │       ├── GetById/           # Get product by ID endpoints
│   │       ├── GetDropdown/       # Dropdown data endpoints
│   │       ├── GetPaginated/      # Paginated list endpoints
│   │       └── ChangeState/       # State change endpoints
│   │
│   └── Examples/                # Example implementations
│       └── ErrorHandlingExampleEndpoint.cs  # Error handling examples
│
├── logs/                       # Application logs (runtime)
├── bin/                        # Compiled binaries
├── obj/                        # Build objects
├── Properties/                 # Project properties
│   └── launchSettings.json       # Development settings
├── appsettings.json           # Application configuration
├── appsettings.Development.json # Development configuration
├── AssemblyReference.cs       # Assembly reference for DI scanning
├── VisitorApp.API.csproj    # Project file
├── VisitorApp.API.http      # HTTP test requests
├── Dockerfile                 # Docker containerization
├── GlobalUsings.cs           # Global using statements
└── Program.cs                # Application entry point
```

### نقش فایل‌های کلیدی

#### Program.cs
```csharp
// Entry point - تنظیمات اولیه و pipeline
var builder = WebApplication.CreateBuilder(args);

// Service registrations
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddMappingServices();
// ... other services

var app = builder.Build();

// Pipeline configuration
app.ConfigurePipeline();
app.Run();
```

#### EndpointBase.cs
کلاس‌های پایه برای ایجاد API endpoints با FastEndpoints framework.

#### Service Extensions
فایل‌های Configuration/ شامل extension methods برای تنظیم services در Program.cs.

## لایه Application (VisitorApp.Application)

### نمای کلی
منطق کاربردی، CQRS patterns و use cases.

```
VisitorApp.Application/
├── Common/                      # کامپوننت‌های مشترک Application
│   ├── Contracts/              # DTOs و Contracts
│   │   ├── DropDownDto.cs        # Dropdown data transfer object
│   │   ├── PaginatedResponse.cs   # Paginated response model
│   │   ├── Pagination.cs          # Pagination parameters
│   │   └── SortingBy.cs          # Sorting parameters
│   │
│   ├── Interfaces/             # Core interfaces
│   │   ├── IReadRepository.cs     # Read repository interface
│   │   ├── IRepository.cs         # Full repository interface
│   │   └── IWriteRepository.cs    # Write repository interface
│   │
│   ├── Messaging/              # CQRS Infrastructure
│   │   ├── IRequestBase.cs        # Base request interface
│   │   ├── IRequestHandlerBase.cs # Base handler interface
│   │   ├── IValidatorService.cs   # Validation service interface
│   │   └── RequestHandlerBase.cs  # Base handler implementation
│   │
│   └── Services/               # Application Services
│       └── ICurrentUserService.cs # Current user service interface
│
├── Features/                   # Feature-based organization
│   └── Catalog/               # Catalog domain
│       ├── MappingProfile.cs    # AutoMapper profile
│       └── Products/          # Product operations
│           ├── Create/          # Product creation
│           │   ├── CreateProductCommandHandler.cs      # Command handler
│           │   ├── CreateProductCommandRequest.cs      # Command request
│           │   ├── CreateProductCommandResponse.cs     # Command response
│           │   └── Validators/  # Request validators
│           │       └── CreateCategoryCommandValidator.cs
│           │
│           ├── Update/          # Product updates
│           │   ├── UpdateGroupCommandHandler.cs
│           │   ├── UpdateGroupCommandRequest.cs
│           │   └── UpdateGroupResponse.cs
│           │
│           ├── Delete/          # Product deletion
│           │   ├── DeleteGroupCommandHandler.cs
│           │   └── DeleteGroupCommandRequest.cs
│           │
│           ├── GetById/         # Get by ID queries
│           │   ├── GetGroupByIdQueryHandler.cs
│           │   ├── GetGroupByIdQueryRequest.cs
│           │   └── GetGroupByIdResponse.cs
│           │
│           ├── GetDropdown/     # Dropdown queries
│           │   ├── GetAllGroupsQueryHandler.cs
│           │   ├── GetAllGroupsQueryRequest.cs
│           │   └── GetAllGroupsResponse.cs
│           │
│           ├── GetPaginated/    # Paginated queries
│           │   ├── GetAllGroupsQueryHandler.cs
│           │   ├── GetAllGroupsQueryRequest.cs
│           │   └── GetAllGroupsResponse.cs
│           │
│           └── ChangeState/     # State changes
│               ├── ChangeStateProductCommandHandler.cs
│               ├── ChangeStateProductCommandRequest.cs
│               └── ChangeStateProductCommandResponse.cs
│
├── bin/                       # Compiled binaries
├── obj/                       # Build objects
├── AssemblyReference.cs       # Assembly reference
├── VisitorApp.Application.csproj # Project file
└── GlobalUsings.cs           # Global using statements
```

### الگوی CQRS

#### Commands (نوشتن)
```csharp
// Handler
public class CreateProductCommandHandler : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>
{
    // Implementation with validation, mapping, and persistence
}

// Request
public class CreateProductCommandRequest : IRequestBase<CreateProductCommandResponse>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Response
public class CreateProductCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
```

#### Queries (خواندن)
```csharp
// Handler
public class GetProductByIdQueryHandler : RequestHandlerBase<GetProductByIdQueryRequest, GetProductByIdResponse>
{
    // Implementation with data retrieval and mapping
}
```

## لایه Domain (VisitorApp.Domain)

### نمای کلی
هسته کسب و کار، موجودات، و قوانین دامین.

```
VisitorApp.Domain/
├── Common/                     # کامپوننت‌های مشترک Domain
│   ├── Contracts/             # Domain interfaces
│   │   ├── IAuditable.cs        # Auditing interface
│   │   ├── IDomainEvent.cs      # Domain events interface
│   │   ├── IEntity.cs           # Entity interface
│   │   ├── IHardDeletable.cs    # Hard delete interface
│   │   └── ISoftDeletable.cs    # Soft delete interface
│   │
│   ├── DTOs/                  # Domain DTOs
│   │   ├── DropDownDto.cs       # Dropdown DTO
│   │   ├── PaginatedResponse.cs # Paginated response
│   │   └── Pagination.cs        # Pagination DTO
│   │
│   ├── Entities/              # Base entity classes
│   │   ├── Entity.cs            # Default entity (Guid-based)
│   │   ├── EntityBase.cs        # Generic entity base
│   │   └── EntityWithAudit.cs   # Entity with audit fields
│   │
│   └── Enums/                 # Domain enumerations (empty)
│
├── Features/                  # Domain features
│   ├── Audit/                 # Audit functionality
│   │   ├── AuditLog.cs          # Audit log entity
│   │   └── Interfaces/
│   │       └── IAuditService.cs # Audit service interface
│   │
│   └── Catalog/              # Catalog domain
│       ├── Entities/
│       │   └── Product.cs       # Product entity
│       └── Interfaces/
│           └── IProductService.cs # Product domain service interface
│
├── Shared/                   # Shared domain concepts
│   ├── Error.cs               # Error representation
│   ├── IValidationResult.cs   # Validation result interface
│   ├── Result.cs             # Result pattern (no return)
│   ├── ResultT.cs           # Result pattern (with return)
│   ├── ValidationResult.cs   # Validation result implementation
│   └── ValidationResultT.cs # Generic validation result
│
├── bin/                      # Compiled binaries
├── obj/                      # Build objects
├── AssemblyReference.cs      # Assembly reference
└── VisitorApp.Domain.csproj # Project file
```

### Entity Hierarchy
```csharp
// Base entity
public abstract class EntityBase<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}

// Audit-enabled entity
public abstract class EntityWithAudit<TKey> : EntityBase<TKey>, IAuditable
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

// Default entity (Guid)
public abstract class Entity : EntityWithAudit<Guid>
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}
```

### Result Pattern
```csharp
// For operations without return value
public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    
    public static Result Success() { }
    public static Result Failure(Error error) { }
}

// For operations with return value
public class Result<T> : Result
{
    public T Value { get; }
    
    public static Result<T> Success(T value) { }
    public static Result<T> Failure(Error error) { }
}
```

## لایه Infrastructure (VisitorApp.Infrastructure)

### نمای کلی
پیاده‌سازی سرویس‌های خارجی و Repository pattern.

```
VisitorApp.Infrastructure/
├── Common/                    # کامپوننت‌های مشترک Infrastructure
│   ├── Helpers/              # Helper utilities
│   │   └── QueryableExtensions.cs # IQueryable extension methods
│   │
│   └── Repository/           # Repository implementations
│       ├── EfRepository.cs     # Entity Framework repository
│       ├── ReadRepository.cs   # Read-only repository
│       └── WriteRepository.cs  # Write repository
│
├── Features/                 # Feature-specific infrastructure
│   └── Audit/               # Audit infrastructure
│       └── Services/
│           └── AuditService.cs # Audit service implementation
│
├── bin/                     # Compiled binaries
├── obj/                     # Build objects
├── AssemblyReference.cs     # Assembly reference
└── VisitorApp.Infrastructure.csproj # Project file
```

### Repository Pattern
```csharp
// Generic repository
public class EfRepository<T, TKey> : WriteRepository<T, TKey>, IRepository<T, TKey>
    where T : EntityBase<TKey>
{
    public EfRepository(ApplicationDbContext db, IAuditService auditService) 
        : base(db, auditService) { }
    
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _db.SaveChangesAsync(cancellationToken);
    }
}

// Default repository (Guid-based)
public class EfRepository<T> : EfRepository<T, Guid>, IRepository<T>
    where T : Entity
{
    public EfRepository(ApplicationDbContext db, IAuditService auditService) 
        : base(db, auditService) { }
}
```

## لایه Persistence (VisitorApp.Persistence)

### نمای کلی
دسترسی به داده، Entity Framework، و تنظیمات دیتابیس.

```
VisitorApp.Persistence/
├── Common/                   # کامپوننت‌های مشترک Persistence
│   └── Context/             # Database context
│       ├── ApplicationDbContext.cs        # Main DbContext
│       └── ApplicationDbContextFactory.cs # DbContext factory
│
├── Features/                # Feature-specific persistence
│   ├── Audit/              # Audit persistence
│   │   └── Configurations/
│   │       └── AuditLogConfiguration.cs # Audit entity configuration
│   │
│   └── Catalog/            # Catalog persistence
│       ├── Constants/        # Catalog constants (empty)
│       └── Products/       # Product persistence
│           ├── Configurations/
│           │   └── ProductConfiguration.cs # Product entity configuration
│           ├── Services/
│           │   └── ProductService.cs      # Product domain service
│           └── Specifications/ # Query specifications (empty)
│
├── Migrations/             # Entity Framework migrations
│   ├── 20250904061647_init.cs           # Initial migration
│   ├── 20250904061647_init.Designer.cs  # Migration designer
│   └── ApplicationDbContextModelSnapshot.cs # Model snapshot
│
├── bin/                   # Compiled binaries
├── obj/                   # Build objects
├── AssemblyReference.cs   # Assembly reference
└── VisitorApp.Persistence.csproj # Project file
```

### Entity Configuration
```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
```

### DbContext
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
```

## پوشه Shared

### نمای کلی
کامپوننت‌های مشترک بین پروژه‌ها.

```
Shared/
├── Common/                  # Common utilities
│   └── PaginatedCommandRequest.cs # Base paginated request
├── Helpers/                # Helper utilities
│   ├── DateTimeHelper.cs     # Date/time utilities
│   ├── EnumExtensions.cs     # Enum extension methods
│   └── StringExtensions.cs   # String extension methods
├── PagedResponse.cs        # Paged response model
├── bin/                   # Compiled binaries
├── obj/                   # Build objects
└── Shared.csproj         # Project file
```

## فایل‌های تنظیمات

### Docker Configuration
```yaml
# docker-compose.yml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5000:80"
    depends_on:
      - database

  database:
    image: postgres:13
    environment:
      POSTGRES_DB: VisitorApp
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: password
```

### Application Settings
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VisitorApp;Trusted_Connection=true;"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  }
}
```

## قوانین سازماندهی

### 1. Feature-Based Organization
- هر feature در پوشه جداگانه
- ساختار مشابه در همه لایه‌ها
- Group related files together

### 2. Layered Architecture
- جداسازی مسئولیت‌ها
- Dependency flow: API → Application → Domain ← Infrastructure ← Persistence

### 3. Naming Conventions
- PascalCase برای folders و files
- Descriptive names
- Consistent patterns across layers

### 4. File Organization
- یک کلاس در یک فایل
- فایل‌های مرتبط در یک پوشه
- Logical grouping by functionality

## مزایای این ساختار

1. **Maintainability**: ساختار واضح و قابل نگهداری
2. **Scalability**: قابل توسعه بدون پیچیدگی
3. **Team Collaboration**: سازماندهی واضح برای تیم
4. **Code Discovery**: پیدا کردن آسان کد مرتبط
5. **Testing**: جداسازی مناسب برای تست

## راهنمای اضافه کردن Feature جدید

### 1. Domain Layer
```
Features/{FeatureName}/
├── Entities/
│   └── {EntityName}.cs
└── Interfaces/
    └── I{EntityName}Service.cs
```

### 2. Application Layer
```
Features/{FeatureName}/
├── MappingProfile.cs
└── {EntityName}/
    ├── Create/
    ├── Update/
    ├── Delete/
    ├── GetById/
    └── GetPaginated/
```

### 3. Infrastructure Layer
```
Features/{FeatureName}/
└── Services/
    └── {EntityName}Service.cs
```

### 4. Persistence Layer
```
Features/{FeatureName}/
├── Configurations/
│   └── {EntityName}Configuration.cs
└── Services/
    └── {EntityName}Service.cs
```

### 5. API Layer
```
Features/{FeatureName}/
├── MappingProfile.cs
└── {EntityName}/
    ├── Create/
    ├── Update/
    ├── Delete/
    ├── GetById/
    └── GetPaginated/
``` 