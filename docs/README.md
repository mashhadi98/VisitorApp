# VisitorApp - مستندات پروژه

## نمای کلی

VisitorApp یک پروژه **Clean Architecture** است که بر اساس بهترین practices و اصول SOLID طراحی شده است. این پروژه از CQRS pattern، Repository pattern و FastEndpoints استفاده می‌کند.

## قوانین اجباری
- قبل از شروع تسک‌ها، اسناد `VisitorApp.Api/docs` را مطالعه کنید.
- پس از اتمام هر تسک، اسناد مرتبط را به‌روزرسانی کنید.
- پس از هر تسک، پروژه را build کنید و خطاها را رفع کنید.

## 📋 فهرست مطالب

### 📚 مستندات اصلی
1. [**نمای کلی معماری**](./01-Architecture-Overview.md) - درک کامل از معماری پروژه
2. [**استانداردهای کد نویسی**](./02-Code-Standards.md) - قوانین و استانداردهای کد نویسی
3. [**ساختار پروژه**](./03-Project-Structure.md) - توضیح کامل ساختار فایل‌ها و پوشه‌ها
4. [**راهنمای توسعه**](./04-Development-Guidelines.md) - فرآیندها و workflows
5. [**قرارداد پاسخ API**](./API-Response-Contract.md) - ساختار استاندارد پاسخ‌ها

## 🚀 شروع سریع

### پیش‌نیازها
- .NET 8.0 SDK یا بالاتر
- SQL Server یا PostgreSQL
- Git
- Docker (اختیاری)

### نصب و راه‌اندازی
```bash
# 1. Clone repository
git clone <repository-url>
cd VisitorApp

# 2. Restore packages
dotnet restore

# 3. Update database
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API

# 4. Run application
cd VisitorApp.API
dotnet run
```

### دسترسی به برنامه
- **API**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

## 🏗️ معماری

این پروژه از **Clean Architecture** با لایه‌های زیر استفاده می‌کند:

```
┌─────────────────────┐
│    API Layer        │ ← FastEndpoints, Middleware, Configuration
├─────────────────────┤
│ Application Layer   │ ← CQRS, MediatR, Validation, Mapping
├─────────────────────┤
│   Domain Layer      │ ← Entities, Business Rules, Interfaces
├─────────────────────┤
│Infrastructure Layer │ ← Repository Implementation, External Services
├─────────────────────┤
│ Persistence Layer   │ ← Entity Framework, Database Configuration
└─────────────────────┘
```

**مزایای این معماری:**
- ✅ جداسازی واضح مسئولیت‌ها
- ✅ تست‌پذیری بالا
- ✅ قابلیت نگهداری آسان
- ✅ توسعه‌پذیری مناسب
- ✅ استقلال از تکنولوژی‌های خارجی

## 🛠️ تکنولوژی‌های استفاده شده

### Core Framework
- **.NET 8** - فریمورک اصلی
- **ASP.NET Core** - وب API
- **Entity Framework Core** - ORM
- **FastEndpoints** - API endpoints

### Patterns & Libraries
- **CQRS** - Command Query Responsibility Segregation
- **MediatR** - Mediator pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging [[memory:8258441]]

### Database
- **SQL Server** / **PostgreSQL** - Database
- **Entity Framework Migrations** - Schema management

### Testing
- **xUnit** / **NUnit** - Unit testing
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library

## 📁 ساختار پروژه

```
VisitorApp/
├── 📂 VisitorApp.API/              # 🌐 Presentation Layer
│   ├── Common/                        # Shared API components
│   ├── Features/                      # Feature-based endpoints
│   └── Program.cs                     # Application entry point
│
├── 📂 VisitorApp.Application/       # 💼 Business Logic Layer
│   ├── Common/                        # Shared application components
│   └── Features/                      # CQRS handlers and DTOs
│
├── 📂 VisitorApp.Domain/            # 🎯 Core Business Layer
│   ├── Common/                        # Base entities and interfaces
│   ├── Features/                      # Domain entities and services
│   └── Shared/                        # Result patterns and errors
│
├── 📂 VisitorApp.Infrastructure/    # 🔧 External Services Layer
│   ├── Common/                        # Repository implementations
│   └── Features/                      # Service implementations
│
├── 📂 VisitorApp.Persistence/       # 💾 Data Access Layer
│   ├── Common/                        # Database context
│   ├── Features/                      # Entity configurations
│   └── Migrations/                    # Database migrations
│
└── 📂 Shared/                        # 🤝 Common Utilities
    └── Helpers/                       # Extension methods and utilities
```

## 🎯 الگوهای طراحی

### 1. CQRS Pattern
- **Commands**: عملیات تغییر state (Create, Update, Delete)
- **Queries**: عملیات خواندن داده (Get, List, Search)
- **Handlers**: پردازش‌کننده‌های درخواست‌ها

### 2. Repository Pattern
- **IRepository<T>**: رابط کامل repository
- **IReadRepository<T>**: عملیات خواندن
- **IWriteRepository<T>**: عملیات نوشتن

### 3. Result Pattern
```csharp
// Success result
var result = Result<Product>.Success(product);

// Failure result  
var result = Result<Product>.Failure(error);

// Usage
if (result.IsSuccess)
    return result.Value;
else
    throw new Exception(result.Error.Message);
```

## 📊 Performance & Monitoring

### Logging با Serilog [[memory:8258441]]
```csharp
_logger.LogInformation("Creating product with name: {ProductName}", request.Name);
_logger.LogError(ex, "Failed to create product: {ProductName}", request.Name);
```

### Database Performance
```csharp
// ✅ Use AsNoTracking for read-only queries
var products = await context.Products
    .AsNoTracking()
    .Where(p => p.IsActive)
    .ToListAsync();

// ✅ Use pagination
var pagedProducts = await context.Products
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

## 📈 CI/CD

### GitHub Actions
- ✅ Automated testing
- ✅ Code coverage reporting
- ✅ Quality gates
- ✅ Automated deployment

### Quality Metrics
- **Code Coverage**: حداقل 80%
- **Maintainability Index**: حداقل 70
- **Cyclomatic Complexity**: حداکثر 10 per method

## 🔧 Development Workflow

### Git Flow
```
main ← develop ← feature/product-management
              ← bugfix/login-issue
              ← hotfix/security-patch
```

### Commit Messages
```bash
feat(products): add product creation endpoint
fix(auth): resolve login validation issue
docs(api): update swagger documentation
refactor(domain): improve entity structure
```

## 📞 Support & Contributing

### اضافه کردن Feature جدید

1. **Domain Layer**: Entity و Interface
2. **Application Layer**: Commands/Queries و Handlers
3. **Infrastructure Layer**: Service Implementation
4. **Persistence Layer**: Entity Configuration
5. **API Layer**: Endpoints

### مراحل Development
1. Create feature branch
2. Implement changes following standards
3. Write comprehensive tests
4. Update documentation
5. Create pull request
6. Code review and merge

## 🔗 لینک‌های مفید

### مستندات داخلی
- [نمای کلی معماری](./01-Architecture-Overview.md) - معماری کامل سیستم
- [استانداردهای کد](./02-Code-Standards.md) - قوانین کدنویسی
- [ساختار پروژه](./03-Project-Structure.md) - سازماندهی فایل‌ها
- [راهنمای توسعه](./04-Development-Guidelines.md) - فرآیندها و workflows
- [قرارداد پاسخ API](./API-Response-Contract.md) - ساختار استاندارد پاسخ‌ها

### مستندات خارجی
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [FastEndpoints](https://fast-endpoints.com/)
- [MediatR](https://github.com/jbogard/MediatR)

## 🏷️ Versioning

این پروژه از [Semantic Versioning](https://semver.org/) استفاده می‌کند:
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes

## 📄 License

این پروژه تحت لیسانس MIT منتشر شده است.

---

**نکته**: این مستندات برای درک بهتر معماری و استانداردهای پروژه طراحی شده‌اند. در صورت سوال یا نیاز به توضیح بیشتر، لطفاً با تیم توسعه تماس بگیرید. 