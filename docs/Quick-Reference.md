# راهنمای سریع (Quick Reference)

## 🚀 دستورات رایج

### Project Setup
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API project
cd VisitorApp.API && dotnet run

# Run tests
dotnet test

# Database update
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API
```

### Git Commands
```bash
# Create feature branch
git checkout develop
git pull origin develop
git checkout -b feature/new-feature

# Commit changes
git add .
git commit -m "feat(feature): add new functionality"

# Push and create PR
git push origin feature/new-feature
```

## 🏗️ الگوهای رایج

### 1. ایجاد Entity جدید

#### Domain Layer
```csharp
// VisitorApp.Domain/Features/YourFeature/Entities/YourEntity.cs
public class YourEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
```

#### Persistence Layer
```csharp
// VisitorApp.Persistence/Features/YourFeature/Configurations/YourEntityConfiguration.cs
public class YourEntityConfiguration : IEntityTypeConfiguration<YourEntity>
{
    public void Configure(EntityTypeBuilder<YourEntity> builder)
    {
        builder.ToTable("YourEntities");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
    }
}
```

#### Add to DbContext
```csharp
// VisitorApp.Persistence/Common/Context/ApplicationDbContext.cs
public DbSet<YourEntity> YourEntities { get; set; }
```

### 2. ایجاد Command Handler

#### Application Layer
```csharp
// Request
public class CreateYourEntityCommandRequest : IRequestBase<CreateYourEntityCommandResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// Response
public class CreateYourEntityCommandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Handler
public class CreateYourEntityCommandHandler(
    IRepository<YourEntity> repository, 
    IMapper mapper,
    IEnumerable<IValidatorService<CreateYourEntityCommandRequest, CreateYourEntityCommandResponse>> validators) 
    : RequestHandlerBase<CreateYourEntityCommandRequest, CreateYourEntityCommandResponse>(validators)
{
    public override async Task<CreateYourEntityCommandResponse> Handler(
        CreateYourEntityCommandRequest request, 
        CancellationToken cancellationToken)
    {
        var entity = mapper.Map<YourEntity>(request);
        
        await repository.AddAsync(entity, autoSave: true, cancellationToken: cancellationToken);
        
        var response = mapper.Map<CreateYourEntityCommandResponse>(entity);
        return response;
    }
}
```

#### Validator
```csharp
public class CreateYourEntityCommandValidator : AbstractValidator<CreateYourEntityCommandRequest>
{
    public CreateYourEntityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters");
    }
}
```

### 3. ایجاد Query Handler

```csharp
// Request
public class GetYourEntityByIdQueryRequest : IRequestBase<GetYourEntityByIdResponse>
{
    public Guid Id { get; set; }
}

// Response
public class GetYourEntityByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Handler
public class GetYourEntityByIdQueryHandler(
    IReadRepository<YourEntity> repository,
    IMapper mapper) 
    : RequestHandlerBase<GetYourEntityByIdQueryRequest, GetYourEntityByIdResponse>
{
    public override async Task<GetYourEntityByIdResponse> Handler(
        GetYourEntityByIdQueryRequest request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        
        if (entity == null)
            throw new NotFoundException($"YourEntity with ID {request.Id} not found");
            
        var response = mapper.Map<GetYourEntityByIdResponse>(entity);
        return response;
    }
}
```

### 4. ایجاد API Endpoint

#### API Layer
```csharp
// Request DTO
public class CreateYourEntityRequest : RequestBase
{
    public override string Route => "/api/your-entities";
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// Endpoint
public class CreateYourEntityEndpoint : EndpointBase<CreateYourEntityRequest, CreateYourEntityCommandRequest, CreateYourEntityCommandResponse>
{
    public override ApiTypes Type => ApiTypes.Post;
    public override string? Summary => "Create new YourEntity";
    public override string? Description => "Creates a new YourEntity with the provided information";

    public CreateYourEntityEndpoint(ISender sender, IMapper mapper) : base(sender, mapper) { }
}
```

### 5. Mapping Profile

```csharp
// VisitorApp.Application/Features/YourFeature/MappingProfile.cs
public class YourFeatureMappingProfile : Profile
{
    public YourFeatureMappingProfile()
    {
        // API Request to Application Command
        CreateMap<CreateYourEntityRequest, CreateYourEntityCommandRequest>();
        
        // Application Command to Domain Entity
        CreateMap<CreateYourEntityCommandRequest, YourEntity>();
        
        // Domain Entity to Application Response
        CreateMap<YourEntity, CreateYourEntityCommandResponse>();
        
        // Domain Entity to Query Response
        CreateMap<YourEntity, GetYourEntityByIdResponse>();
    }
}
```

## 📊 Database Operations

### Migration Commands
```bash
# Add new migration
dotnet ef migrations add YourMigrationName --project VisitorApp.Persistence --startup-project VisitorApp.API

# Remove last migration (before applying)
dotnet ef migrations remove --project VisitorApp.Persistence --startup-project VisitorApp.API

# Update database
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API

# Generate SQL script
dotnet ef migrations script --project VisitorApp.Persistence --startup-project VisitorApp.API

# Update to specific migration
dotnet ef database update YourMigrationName --project VisitorApp.Persistence --startup-project VisitorApp.API
```

### Repository Usage
```csharp
// Read operations
var entity = await repository.GetByIdAsync(id);
var entities = await repository.GetAllAsync();
var count = await repository.CountAsync();

// Write operations
await repository.AddAsync(entity, autoSave: true);
await repository.UpdateAsync(entity, autoSave: true);
await repository.DeleteAsync(id, autoSave: true);

// Batch operations
await repository.AddRangeAsync(entities, autoSave: true);
await repository.UpdateRangeAsync(entities, autoSave: true);
await repository.DeleteRangeAsync(ids, autoSave: true);
```

## 🧪 تست‌نویسی

### Unit Test Template
```csharp
[TestClass]
public class YourHandlerTests
{
    private Mock<IRepository<YourEntity>> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private YourHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<YourEntity>>();
        _mapperMock = new Mock<IMapper>();
        _handler = new YourHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handler_ValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var request = new YourRequest { Name = "Test" };
        var entity = new YourEntity { Id = Guid.NewGuid(), Name = "Test" };
        var expectedResponse = new YourResponse { Id = entity.Id, Name = "Test" };

        _mapperMock.Setup(m => m.Map<YourEntity>(request)).Returns(entity);
        _mapperMock.Setup(m => m.Map<YourResponse>(entity)).Returns(expectedResponse);
        _repositoryMock.Setup(r => r.AddAsync(entity, true, It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handler(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedResponse.Name, result.Name);
        _repositoryMock.Verify(r => r.AddAsync(entity, true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task Handler_NullRequest_ShouldThrowException()
    {
        // Act & Assert
        await _handler.Handler(null, CancellationToken.None);
    }
}
```

### Integration Test Template
```csharp
[TestClass]
public class YourEndpointTests : IntegrationTestBase
{
    [TestMethod]
    public async Task CreateYourEntity_ValidRequest_ShouldReturn201()
    {
        // Arrange
        var request = new { Name = "Test Entity", Description = "Test Description" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/your-entities", request);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<CreateYourEntityResponse>();
        Assert.IsNotNull(result);
        Assert.AreEqual(request.Name, result.Name);
    }

    [TestMethod]
    public async Task GetYourEntity_ExistingId_ShouldReturn200()
    {
        // Arrange - Create entity first
        var createRequest = new { Name = "Test Entity", Description = "Test Description" };
        var createResponse = await Client.PostAsJsonAsync("/api/your-entities", createRequest);
        var createdEntity = await createResponse.Content.ReadFromJsonAsync<CreateYourEntityResponse>();

        // Act
        var response = await Client.GetAsync($"/api/your-entities/{createdEntity.Id}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<GetYourEntityByIdResponse>();
        Assert.IsNotNull(result);
        Assert.AreEqual(createdEntity.Id, result.Id);
    }
}
```

## 🔧 Configuration

### appsettings.json Template
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VisitorApp;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day"
        }
      },
      {
        "Name": "Console"
      }
    ]
  },
  "AllowedHosts": "*"
}
```

### Service Registration
```csharp
// Program.cs extension usage
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddMappingServices();
builder.Services.AddCorsServices();
builder.Services.AddLoggingServices();
builder.Services.AddDependencyInjectionServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiDocumentationServices();
```

## 🔍 Debugging & Troubleshooting

### Common Issues

#### 1. Migration Error
```bash
# Problem: Migration already exists
# Solution:
dotnet ef migrations remove --project VisitorApp.Persistence --startup-project VisitorApp.API

# Problem: Database out of sync
# Solution:
dotnet ef database drop --project VisitorApp.Persistence --startup-project VisitorApp.API
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API
```

#### 2. Dependency Injection Error
```csharp
// Problem: Service not registered
// Solution: Check DependencyInjectionServiceExtensions.cs

// Problem: Circular dependency
// Solution: Use interfaces and check your dependency graph
```

#### 3. Validation Error
```csharp
// Problem: Validation not working
// Solution: Ensure validator is registered and inherited from AbstractValidator<T>

// Check if validator is being called in RequestHandlerBase
if (_validators != null && _validators.Any())
{
    foreach (var validator in _validators)
    {
        validator.Execute(request);
    }
}
```

### Logging Commands
```csharp
// Different log levels
_logger.LogTrace("Detailed trace information");
_logger.LogDebug("Debug information for troubleshooting");
_logger.LogInformation("General information about application flow");
_logger.LogWarning("Something unexpected but not an error");
_logger.LogError(exception, "An error occurred");
_logger.LogCritical(exception, "Critical error that may cause application to stop");

// Structured logging
_logger.LogInformation("User {UserId} created product {ProductId} with name {ProductName}", 
    userId, productId, productName);
```

## 📝 Code Snippets

### Exception Handling
```csharp
// Custom exceptions
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// Usage in handlers
if (entity == null)
    throw new NotFoundException($"Entity with ID {request.Id} not found");
```

### Result Pattern Usage
```csharp
// Success result
return Result<YourEntity>.Success(entity);

// Failure result
return Result<YourEntity>.Failure(new Error("ENTITY_NOT_FOUND", "Entity not found"));

// Using result
var result = await someMethod();
if (result.IsSuccess)
{
    var entity = result.Value;
    // Process entity
}
else
{
    var error = result.Error;
    // Handle error
}
```

### AutoMapper Configuration
```csharp
// Conditional mapping
CreateMap<Source, Destination>()
    .ForMember(dest => dest.Property, opt => opt.MapFrom(src => 
        src.SomeCondition ? src.ValueA : src.ValueB));

// Custom value resolver
CreateMap<Source, Destination>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom<FullNameResolver>());

public class FullNameResolver : IValueResolver<Source, Destination, string>
{
    public string Resolve(Source source, Destination destination, string destMember, ResolutionContext context)
    {
        return $"{source.FirstName} {source.LastName}";
    }
}
```

## 🚀 Performance Tips

### Database Optimization
```csharp
// Use AsNoTracking for read-only queries
var products = await context.Products
    .AsNoTracking()
    .Where(p => p.IsActive)
    .ToListAsync();

// Use projection to select only needed fields
var productDtos = await context.Products
    .Where(p => p.IsActive)
    .Select(p => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price
    })
    .ToListAsync();

// Use pagination for large datasets
var pagedProducts = await context.Products
    .Where(p => p.IsActive)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### Memory Management
```csharp
// Use ConfigureAwait(false) in library code
await repository.GetByIdAsync(id).ConfigureAwait(false);

// Dispose resources properly
using var stream = new MemoryStream();
await using var dbContext = new ApplicationDbContext(options);
```

## 📋 Checklist برای Feature جدید

- [ ] ✅ Domain Entity created
- [ ] ✅ Entity Configuration added
- [ ] ✅ DbSet added to DbContext
- [ ] ✅ Migration created and applied
- [ ] ✅ Repository interfaces implemented
- [ ] ✅ Command/Query handlers created
- [ ] ✅ Validators implemented
- [ ] ✅ AutoMapper profiles configured
- [ ] ✅ API endpoints created
- [ ] ✅ Unit tests written
- [ ] ✅ Integration tests written
- [ ] ✅ Documentation updated

---

**نکته**: این راهنمای سریع برای استفاده روزانه طراحی شده است. برای اطلاعات کامل‌تر، لطفاً به [مستندات اصلی](./README.md) مراجعه کنید. 