# استانداردهای کد نویسی (Code Standards)

## نمای کلی

این سند استانداردها و بهترین شیوه‌های کد نویسی در پروژه را تعریف می‌کند. پیروی از این استانداردها باعث یکنواختی، خوانایی و قابلیت نگهداری بهتر کد خواهد شد.

## استانداردهای نام‌گذاری

### کلاس‌ها و Interfaces
```csharp
// ✅ درست - استفاده از PascalCase
public class ProductService
public interface IProductService
public class CreateProductHandler

// ❌ غلط 
public class productService
public class Product_Service
```

### متدها و Properties
```csharp
// ✅ درست - استفاده از PascalCase
public class Product
{
    public string ProductName { get; set; }
    public void UpdateStatus() { }
    public async Task<Result> CreateAsync() { }
}
```

### متغیرهای محلی و پارامترها
```csharp
// ✅ درست - استفاده از camelCase
public void ProcessOrder(int orderId, string customerName)
{
    var productList = new List<Product>();
    var isProcessed = false;
    // ... existing code ...
}
```

### نام‌گذاری سرویس‌ها
```csharp
// ✅ درست - استفاده از نام کامل
public interface ITextToSpeechService  // [[memory:6620681]]
public class ProductManagementService

// ❌ غلط - اختصار
public interface ITtsService
public interface IProdMgmtService
```

### استفاده از var
```csharp
// ✅ درست - همیشه از var استفاده کنید
var product = new Product();
var products = await repository.GetAllAsync();
var isValid = ValidateProduct(product);

// ❌ غلط - مگر برای وضوح نوع لازم باشد
Product product = new Product();
List<Product> products = new List<Product>();
```

## ساختار فایل‌ها و پوشه‌ها

### ساختار Feature
```
Features/
├── Catalog/
│   ├── MappingProfile.cs
│   └── Products/
│       ├── Create/
│       │   ├── CreateProductHandler.cs
│       │   ├── CreateProductRequest.cs
│       │   ├── CreateProductResponse.cs
│       │   └── Validators/
│       │       └── CreateProductValidator.cs
│       ├── Update/
│       └── Delete/
```

### نام‌گذاری فایل‌ها
```
// Command/Query Handlers
{Feature}{Action}CommandHandler.cs
{Feature}{Action}QueryHandler.cs

// Request/Response Models  
{Feature}{Action}CommandRequest.cs
{Feature}{Action}QueryRequest.cs
{Feature}{Action}Response.cs

// Validators
{Feature}{Action}CommandValidator.cs

// Examples:
CreateProductCommandHandler.cs
GetProductByIdQueryHandler.cs
UpdateProductCommandValidator.cs
```

## SOLID Principles Implementation

### 1. Single Responsibility Principle (SRP)
```csharp
// ✅ درست - هر کلاس یک مسئولیت
public class ProductValidator
{
    public ValidationResult ValidateProduct(Product product) { }
}

public class ProductRepository
{
    public async Task<Product> GetByIdAsync(Guid id) { }
    public async Task AddAsync(Product product) { }
}

// ❌ غلط - چندین مسئولیت
public class ProductManager
{
    public ValidationResult ValidateProduct(Product product) { } // Validation
    public async Task<Product> GetByIdAsync(Guid id) { }        // Data Access
    public string FormatProductName(string name) { }            // Formatting
}
```

### 2. Open/Closed Principle (OCP)
```csharp
// ✅ درست - استفاده از Interface برای توسعه‌پذیری
public interface INotificationService
{
    Task SendAsync(string message);
}

public class EmailNotificationService : INotificationService
{
    public async Task SendAsync(string message) { }
}

public class SmsNotificationService : INotificationService
{
    public async Task SendAsync(string message) { }
}
```

### 3. Liskov Substitution Principle (LSP)
```csharp
// ✅ درست - کلاس‌های مشتق شده قابل جایگزینی با کلاس پایه
public abstract class EntityBase<TKey>
{
    public virtual TKey Id { get; set; }
    public virtual void SetId(TKey id) => Id = id;
}

public class Product : EntityBase<Guid>
{
    // Override behavior compatible با کلاس پایه
    public override void SetId(Guid id) 
    {
        if (id == Guid.Empty)
            Id = Guid.NewGuid();
        else
            Id = id;
    }
}
```

### 4. Interface Segregation Principle (ISP)
```csharp
// ✅ درست - Interface های کوچک و مختص
public interface IReadRepository<T>
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
}

public interface IWriteRepository<T>
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

// ❌ غلط - Interface بزرگ
public interface IRepository<T>
{
    // Read operations
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    
    // Write operations
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    
    // Statistics operations
    Task<int> CountAsync();
    Task<decimal> GetAverageAsync();
    
    // Export operations
    Task<byte[]> ExportToCsvAsync();
    Task<string> ExportToJsonAsync();
}
```

### 5. Dependency Inversion Principle (DIP)
```csharp
// ✅ درست - وابستگی به Abstraction
public class ProductService
{
    private readonly IRepository<Product> _repository;
    private readonly IMapper _mapper;
    
    public ProductService(IRepository<Product> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}

// ❌ غلط - وابستگی مستقیم به Concrete class
public class ProductService
{
    private readonly EfRepository<Product> _repository; // مستقیم به EF
    
    public ProductService()
    {
        _repository = new EfRepository<Product>(); // ایجاد مستقیم
    }
}
```

## CQRS Pattern Implementation

### Command Handler Structure
```csharp
public class CreateProductCommandHandler : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IRepository<Product> _repository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IRepository<Product> repository, 
        IMapper mapper,
        IEnumerable<IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>> validators) 
        : base(validators)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<CreateProductCommandResponse> Handler(
        CreateProductCommandRequest request, 
        CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);
        
        await _repository.AddAsync(product, autoSave: true, cancellationToken: cancellationToken);
        
        var response = _mapper.Map<CreateProductCommandResponse>(product);
        return response;
    }
}
```

### Query Handler Structure
```csharp
public class GetProductByIdQueryHandler : RequestHandlerBase<GetProductByIdQueryRequest, GetProductByIdResponse>
{
    private readonly IReadRepository<Product> _repository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IReadRepository<Product> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<GetProductByIdResponse> Handler(
        GetProductByIdQueryRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        
        if (product == null)
            throw new NotFoundException($"Product with ID {request.Id} not found");
            
        var response = _mapper.Map<GetProductByIdResponse>(product);
        return response;
    }
}
```

## Expression-Bodied Members

### استفاده برای متدها و Properties کوتاه
```csharp
// ✅ درست - برای متدهای کوتاه
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    // Expression-bodied property
    public bool IsExpensive => Price > 100;
    
    // Expression-bodied method
    public string GetFormattedPrice() => $"${Price:F2}";
    
    // Expression-bodied method with parameters
    public decimal CalculateDiscount(decimal percentage) => Price * (percentage / 100);
}

// ❌ غلط - برای متدهای پیچیده
public decimal CalculateComplexPrice() => 
    Price * GetTax() + GetShipping() - GetDiscount() + GetInsurance();
```

## String Interpolation

### استفاده از $"" به جای concatenation
```csharp
// ✅ درست - String interpolation
public string GetProductDescription(string name, decimal price, int quantity)
{
    return $"Product: {name}, Price: ${price:F2}, Quantity: {quantity}";
}

public string GetOrderSummary(Order order)
{
    return $"Order #{order.Id} for {order.Customer.Name} - Total: ${order.Total:F2}";
}

// ❌ غلط - String concatenation
public string GetProductDescription(string name, decimal price, int quantity)
{
    return "Product: " + name + ", Price: $" + price.ToString("F2") + ", Quantity: " + quantity;
}
```

## Primary Constructors

### استفاده در کلاس‌ها و Services
```csharp
// ✅ درست - Primary constructor برای dependency injection
public class ProductService(IRepository<Product> repository, IMapper mapper, ILogger<ProductService> logger)
{
    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        logger.LogInformation("Creating product with name: {ProductName}", request.Name);
        
        var product = mapper.Map<Product>(request);
        await repository.AddAsync(product);
        
        return mapper.Map<ProductDto>(product);
    }
}

// ✅ درست - Primary constructor برای handlers
public class CreateProductCommandHandler(
    IRepository<Product> repository, 
    IMapper mapper,
    IEnumerable<IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>> validators) 
    : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>(validators)
{
    // Implementation...
}
```

## Collection Handling

### Return Empty Collections instead of null
```csharp
// ✅ درست - Return empty collection
public async Task<IEnumerable<Product>> GetProductsAsync()
{
    var products = await _repository.GetAllAsync();
    
    return products ?? new List<Product>(); // یا []
}

public List<string> GetProductCategories()
{
    if (someCondition)
        return new List<string>();
        
    return categories;
}

// ❌ غلط - Return null
public async Task<IEnumerable<Product>> GetProductsAsync()
{
    var products = await _repository.GetAllAsync();
    
    if (products == null)
        return null; // ❌
        
    return products;
}
```

## Validation Standards

### FluentValidation Usage
```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommandRequest>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required");
    }
}
```

## Logging Standards

### استفاده از Serilog [[memory:8258441]]
```csharp
public class ProductService
{
    private readonly ILogger<ProductService> _logger;

    public ProductService(ILogger<ProductService> logger)
    {
        _logger = logger;
    }

    public async Task<Product> CreateAsync(CreateProductRequest request)
    {
        _logger.LogInformation("Creating product with name: {ProductName}", request.Name);
        
        try
        {
            // Business logic
            var product = new Product { Name = request.Name };
            
            _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product: {ProductName}", request.Name);
            throw;
        }
    }
}
```

## Error Handling

### Result Pattern Usage
```csharp
// Return Result pattern instead of throwing exceptions for business logic
public async Task<Result<Product>> CreateProductAsync(CreateProductRequest request)
{
    if (string.IsNullOrEmpty(request.Name))
        return Result<Product>.Failure("Product name is required");

    if (request.Price <= 0)
        return Result<Product>.Failure("Price must be greater than zero");

    try
    {
        var product = new Product { Name = request.Name, Price = request.Price };
        await _repository.AddAsync(product);
        
        return Result<Product>.Success(product);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to create product");
        return Result<Product>.Failure("Failed to create product");
    }
}
```

## Performance Best Practices

### Async/Await Usage
```csharp
// ✅ درست - Proper async/await
public async Task<IEnumerable<Product>> GetProductsAsync()
{
    return await _repository.GetAllAsync();
}

public async Task<Product> GetProductByIdAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ غلط - Blocking async calls
public IEnumerable<Product> GetProducts()
{
    return _repository.GetAllAsync().Result; // Deadlock risk
}
```

### Entity Framework Best Practices
```csharp
// ✅ درست - Use AsNoTracking for read-only queries
public async Task<IEnumerable<ProductDto>> GetProductsForDisplayAsync()
{
    var products = await _context.Products
        .AsNoTracking()
        .Select(p => new ProductDto 
        { 
            Id = p.Id, 
            Name = p.Name, 
            Price = p.Price 
        })
        .ToListAsync();
        
    return products;
}
```

## Testing Standards

### Unit Test Structure
```csharp
public class CreateProductHandlerTests
{
    [Test]
    public async Task Handler_ValidRequest_ShouldCreateProduct()
    {
        // Arrange
        var repository = new Mock<IRepository<Product>>();
        var mapper = new Mock<IMapper>();
        var handler = new CreateProductCommandHandler(repository.Object, mapper.Object, validators);
        
        var request = new CreateProductCommandRequest { Name = "Test Product" };
        
        // Act
        var result = await handler.Handler(request, CancellationToken.None);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        repository.Verify(r => r.AddAsync(It.IsAny<Product>(), true, CancellationToken.None), Times.Once);
    }
}
```

## Code Documentation

### XML Documentation
```csharp
/// <summary>
/// Creates a new product in the system
/// </summary>
/// <param name="request">The product creation request containing product details</param>
/// <param name="cancellationToken">Cancellation token for the operation</param>
/// <returns>The created product response with generated ID</returns>
/// <exception cref="ValidationException">Thrown when the request validation fails</exception>
public async Task<CreateProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
{
    // Implementation...
}
```

## کنترل کیفیت

### Pre-commit Checks
- Code formatting با EditorConfig
- Static analysis با SonarQube یا Roslyn analyzers  
- Unit test coverage حداقل 80%
- Security scan با tools مناسب

### Code Review Guidelines
- تمام تغییرات باید از طریق Pull Request انجام شود
- حداقل یک نفر review کند
- تمام tests باید pass شوند
- Documentation بروزرسانی شود 