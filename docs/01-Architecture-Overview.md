# نمای کلی معماری پروژه (Architecture Overview)

## نمای کلی

این پروژه بر اساس **Clean Architecture** طراحی شده است که از اصول SOLID و separation of concerns پیروی می‌کند. معماری شامل پنج لایه اصلی است که هر کدام مسئولیت‌های مشخصی دارند.

## ساختار معماری

```
┌─────────────────────┐
│    API Layer        │ ← Controllers/Endpoints + FastEndpoints
├─────────────────────┤
│ Application Layer   │ ← Business Logic + CQRS + MediatR
├─────────────────────┤
│   Domain Layer      │ ← Entities + Business Rules + Interfaces
├─────────────────────┤
│Infrastructure Layer │ ← Services + External Dependencies
├─────────────────────┤
│ Persistence Layer   │ ← Data Access + EF Core + Repositories
└─────────────────────┘
```

## لایه‌های معماری

### 1. Domain Layer (لایه دامین)
- **مسئولیت**: موجودات (Entities)، قوانین تجاری، و رابط‌ها
- **ویژگی‌ها**:
  - مستقل از تکنولوژی‌های خارجی
  - شامل تمامی قوانین تجاری کور
  - تعریف انتیتی‌ها و ارزش‌ها
  - تعریف رابط‌های اصلی

**فایل‌های کلیدی:**
- `Common/Entities/Entity.cs` - کلاس پایه موجودات
- `Common/Contracts/` - رابط‌های اصلی
- `Features/*/Entities/` - موجودات دامین
- `Features/*/Interfaces/` - رابط‌های سرویس‌ها

### 2. Application Layer (لایه اپلیکیشن)
- **مسئولیت**: منطق کاربردی، CQRS patterns، Use Cases
- **ویژگی‌ها**:
  - پیاده‌سازی الگوی CQRS (Command Query Responsibility Segregation)
  - استفاده از MediatR برای پردازش درخواست‌ها
  - Validation و Business Logic
  - Mapping بین لایه‌ها

**فایل‌های کلیدی:**
- `Common/Messaging/RequestHandlerBase.cs` - کلاس پایه handlers
- `Features/*/Commands/` - Command handlers
- `Features/*/Queries/` - Query handlers
- `Features/*/Validators/` - اعتبارسنجی‌ها

### 3. Infrastructure Layer (لایه زیرساخت)
- **مسئولیت**: پیاده‌سازی سرویس‌های خارجی، Repository Pattern
- **ویژگی‌ها**:
  - Repository Pattern implementation
  - External services integration
  - Cross-cutting concerns

**فایل‌های کلیدی:**
- `Common/Repository/EfRepository.cs` - پیاده‌سازی Repository
- `Features/*/Services/` - سرویس‌های خارجی

### 4. Persistence Layer (لایه پایداری)
- **مسئولیت**: دسترسی به داده، Entity Framework تنظیمات
- **ویژگی‌ها**:
  - Entity Framework Core configurations
  - Database context
  - Migrations
  - Specifications pattern

**فایل‌های کلیدی:**
- `Common/Context/ApplicationDbContext.cs` - DbContext اصلی
- `Features/*/Configurations/` - Entity configurations
- `Migrations/` - مایگریشن‌های دیتابیس

### 5. API Layer (لایه API)
- **مسئولیت**: HTTP endpoints، Authentication، Middleware
- **ویژگی‌ها**:
  - FastEndpoints framework
  - Endpoint routing
  - Middleware pipeline
  - Service configurations

**فایل‌های کلیدی:**
- `Common/Endpoints/EndpointBase.cs` - کلاس پایه endpoints
- `Common/Configuration/` - تنظیمات سرویس‌ها
- `Features/*/Endpoints/` - API endpoints

## الگوهای طراحی استفاده شده

### 1. CQRS (Command Query Responsibility Segregation)
- جداسازی عملیات خواندن از نوشتن
- Commands برای تغییر state
- Queries برای خواندن داده

### 2. Repository Pattern
- انتزاع لایه دسترسی به داده
- `IRepository<T>` و `IReadRepository<T>`
- EF Core implementation

### 3. Mediator Pattern
- استفاده از MediatR
- Decoupling بین لایه‌ها
- Pipeline behaviors

### 4. Factory Pattern
- ApplicationDbContextFactory
- Service factories

### 5. Specification Pattern
- Query specifications
- Business rule encapsulation

## مدیریت وابستگی‌ها

### Dependency Injection
- استفاده از built-in DI container
- Automatic service registration با Scrutor
- Different lifetimes (Scoped, Transient, Singleton)

### Service Registration Strategy
```csharp
// Transient برای Validators
services.Scan(selector => selector
    .AddClasses(classes => classes.AssignableTo(typeof(IValidatorService<>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

// Scoped برای سایر سرویس‌ها
services.Scan(selector => selector
    .AddClasses(classes => classes.NotInNamespaces("VisitorApp.Persistence.Specifications"))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
```

## Cross-Cutting Concerns

### Logging
- استفاده از Serilog [[memory:8258441]]
- Structured logging
- LogUserInfoMiddleware برای audit

### Validation
- FluentValidation در Application layer
- RequestHandlerBase integration
- Automatic validation pipeline

### Mapping
- AutoMapper برای object mapping
- Profile-based configurations
- Automatic registration

### Error Handling
- Result pattern implementation
- Custom error responses
- Exception handling middleware

## Database Strategy

### Entity Framework Core
- Code First approach
- Automatic migrations
- Configuration-based entity setup

### Audit Trail
- IAuditable interface
- AuditLog entity
- Automatic audit service

## Security

### Authentication & Authorization
- Role-based authorization
- Endpoint-level security
- JWT token support (قابل تنظیم)

## Performance Considerations

### Query Optimization
- Specification pattern
- IQueryable extensions
- Pagination support

### Caching Strategy
- قابل توسعه برای Redis
- Memory caching

## Testing Strategy

### Unit Testing
- Handler testing
- Repository testing
- Service testing

### Integration Testing
- Database testing
- API endpoint testing

## Deployment

### Docker Support
- Multi-stage Dockerfile
- Docker Compose configuration
- Environment-specific settings

## مزایای این معماری

1. **Testability**: جداسازی واضح لایه‌ها
2. **Maintainability**: کد قابل نگهداری و توسعه
3. **Scalability**: قابلیت مقیاس‌پذیری
4. **Flexibility**: انعطاف‌پذیری در تغییرات
5. **Independence**: استقلال لایه‌ها از یکدیگر

## جهت‌گیری‌های آتی

- Microservices readiness
- Event-driven architecture
- CQRS with Event Sourcing
- Distributed caching
- Message queuing integration 