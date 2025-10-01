# راهنمای توسعه (Development Guidelines)

## قوانین اجباری جریان کار
- قبل از شروع هر تسک: مستندات `VisitorApp.Api/docs` را مرور کنید (حداقل: `README.md`, `01-Architecture-Overview.md`, `03-Project-Structure.md`, `04-Development-Guidelines.md`).
- پس از اتمام هر تسک: مستندات مرتبط را به‌روزرسانی کنید (Endpoints، تنظیمات، مهاجرت‌ها).
- پس از هر تسک: پروژه را build کنید و خطاها را رفع کنید:
```bash
dotnet restore
dotnet build VisitorApp.sln -c Release
dotnet test --no-build -c Release # در صورت وجود تست
```

## نمای کلی

این سند فرآیندها، ابزارها و راهنمای قدم به قدم برای توسعه در این پروژه را ارائه می‌دهد. هدف تضمین کیفیت، سازگاری و بهره‌وری در فرآیند توسعه است.

## محیط توسعه (Development Environment)

### پیش‌نیازها

#### ابزارهای ضروری
```bash
# .NET SDK (حداقل نسخه 8.0)
dotnet --version

# Git
git --version

# Docker (اختیاری برای local database)
docker --version
```

#### IDE های پیشنهادی
- **Visual Studio 2022** (Windows)
- **Visual Studio Code** (Cross-platform)
- **JetBrains Rider** (Cross-platform)

#### Extensions مفید برای VS Code
```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.vscode-dotnet-runtime",
    "formulahendry.auto-rename-tag",
    "bradlc.vscode-tailwindcss",
    "ms-vscode.vscode-json"
  ]
}
```

### تنظیمات اولیه پروژه

#### 1. Clone کردن Repository
```bash
git clone <repository-url>
cd VisitorApp
```

#### 2. Restore Dependencies
```bash
dotnet restore
```

#### 3. Database Setup
```bash
# Update connection string in appsettings.Development.json
# Run migrations
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API
```

#### 4. Run Application
```bash
cd VisitorApp.API
dotnet run
```

## فرآیند توسعه (Development Workflow)

### Git Workflow

#### Branch Strategy
```
main                    # Production branch
├── develop            # Integration branch
├── feature/           # Feature branches
│   ├── feature/user-management
│   ├── feature/product-catalog
│   └── feature/order-processing
├── bugfix/           # Bug fix branches
│   ├── bugfix/login-issue
│   └── bugfix/payment-error
└── hotfix/          # Critical fixes
    └── hotfix/security-patch
```

#### Naming Conventions
```bash
# Features
feature/product-management
feature/user-authentication
feature/order-processing

# Bug fixes
bugfix/login-validation
bugfix/payment-calculation

# Hotfixes
hotfix/security-vulnerability
hotfix/performance-issue
```

#### Commit Message Format
```bash
# Format: <type>(<scope>): <description>

# Types:
feat: new feature
fix: bug fix
docs: documentation
style: formatting
refactor: code restructuring
test: adding tests
chore: maintenance

# Examples:
feat(products): add product creation endpoint
fix(auth): resolve login validation issue
docs(api): update swagger documentation
refactor(domain): improve entity structure
```

### Feature Development Process

#### 1. شروع Feature جدید
```bash
# Create and switch to feature branch
git checkout develop
git pull origin develop
git checkout -b feature/product-management

# Update your branch regularly
git fetch origin
git rebase origin/develop
```

#### 2. Development Steps

##### Step 1: Domain Layer
```csharp
// Create entity
public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}

// Create domain service interface
public interface IProductService
{
    Task<Result<Product>> CreateAsync(Product product);
    Task<Result<Product>> UpdateAsync(Product product);
}
```

##### Step 2: Application Layer
```csharp
// Create command/query handlers
public class CreateProductCommandHandler : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>
{
    // Implementation
}

// Create requests/responses
public class CreateProductCommandRequest : IRequestBase<CreateProductCommandResponse>
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

##### Step 3: Infrastructure/Persistence Layer
```csharp
// Entity configuration
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
    }
}

// Add to DbContext
public DbSet<Product> Products { get; set; }
```

##### Step 4: API Layer
```csharp
// Create endpoint
public class CreateProductEndpoint : EndpointBase<CreateProductRequest, CreateProductCommandRequest, CreateProductResponse>
{
    public override ApiTypes Type => ApiTypes.Post;
    public override string? Summary => "Create new product";
    
    public CreateProductEndpoint(ISender sender, IMapper mapper) : base(sender, mapper) { }
}
```

#### 3. Testing Strategy

##### Unit Tests
```csharp
[TestClass]
public class CreateProductHandlerTests
{
    private Mock<IRepository<Product>> _repositoryMock;
    private Mock<IMapper> _mapperMock;
    private CreateProductCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateProductCommandHandler(_repositoryMock.Object, _mapperMock.Object, new List<IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>>());
    }

    [TestMethod]
    public async Task Handler_ValidRequest_ShouldCreateProduct()
    {
        // Arrange
        var request = new CreateProductCommandRequest { Name = "Test Product", Price = 100 };
        var product = new Product { Id = Guid.NewGuid(), Name = "Test Product", Price = 100 };
        
        _mapperMock.Setup(m => m.Map<Product>(request)).Returns(product);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>(), true, It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handler(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>(), true, It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

##### Integration Tests
```csharp
[TestClass]
public class ProductEndpointsTests : IntegrationTestBase
{
    [TestMethod]
    public async Task CreateProduct_ValidRequest_ShouldReturn201()
    {
        // Arrange
        var request = new CreateProductRequest 
        { 
            Name = "Integration Test Product", 
            Price = 150 
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/products", request);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var product = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
        Assert.IsNotNull(product);
        Assert.AreEqual(request.Name, product.Name);
    }
}
```

#### 4. Code Review Process

##### Pre-Review Checklist
- [ ] تمام unit tests pass شده‌اند
- [ ] Integration tests نوشته شده‌اند
- [ ] Code coverage حداقل 80% است
- [ ] Documentation بروزرسانی شده است
- [ ] استانداردهای کد رعایت شده‌اند
- [ ] Performance implications بررسی شده‌اند
- [ ] Security concerns addressed شده‌اند

##### Pull Request Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Comments added to complex code
- [ ] Documentation updated
- [ ] No breaking changes (or marked as such)
```

### Database Migrations

#### ایجاد Migration جدید
```bash
# Add migration
dotnet ef migrations add AddProductTable --project VisitorApp.Persistence --startup-project VisitorApp.API

# Update database
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API

# Generate SQL script
dotnet ef migrations script --project VisitorApp.Persistence --startup-project VisitorApp.API
```

#### Migration Best Practices
```csharp
// Good - Backward compatible
public partial class AddProductTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Name = table.Column<string>(maxLength: 200, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false),
                CreatedBy = table.Column<string>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Products");
    }
}
```

## استانداردهای کد (Code Standards)

### فرمت کد

#### EditorConfig (.editorconfig)
```ini
root = true

[*]
charset = utf-8
end_of_line = crlf
indent_style = space
indent_size = 4
insert_final_newline = true
trim_trailing_whitespace = true

[*.{cs}]
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
```

#### Analyzer Rules (.editorconfig)
```ini
# CA rules
dotnet_diagnostic.CA1001.severity = warning
dotnet_diagnostic.CA1002.severity = warning
dotnet_diagnostic.CA1003.severity = warning

# StyleCop rules
dotnet_diagnostic.SA1600.severity = none # XML documentation
dotnet_diagnostic.SA1633.severity = none # File header
```

### Logging Standards

#### Structured Logging با Serilog [[memory:8258441]]
```csharp
public class ProductService
{
    private readonly ILogger<ProductService> _logger;

    public async Task<Result<Product>> CreateAsync(CreateProductRequest request)
    {
        using var scope = _logger.BeginScope("Creating product {ProductName}", request.Name);
        
        _logger.LogInformation("Starting product creation with data {@ProductData}", request);
        
        try
        {
            var product = new Product 
            { 
                Name = request.Name, 
                Price = request.Price 
            };
            
            await _repository.AddAsync(product);
            
            _logger.LogInformation("Product created successfully with ID {ProductId}", product.Id);
            
            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create product {ProductName}", request.Name);
            return Result<Product>.Failure(new Error("PRODUCT_CREATION_FAILED", "Failed to create product"));
        }
    }
}
```

#### Logging Configuration
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        }
      },
      {
        "Name": "Console",
        "Args": {
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  }
}
```

### Performance Guidelines

#### Database Performance
```csharp
// ✅ Good - Use AsNoTracking for read-only queries
public async Task<IEnumerable<ProductDto>> GetProductsAsync()
{
    return await _context.Products
        .AsNoTracking()
        .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price })
        .ToListAsync();
}

// ✅ Good - Use pagination
public async Task<PaginatedResponse<ProductDto>> GetProductsAsync(int page, int pageSize)
{
    var query = _context.Products.AsNoTracking();
    
    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price })
        .ToListAsync();
    
    return new PaginatedResponse<ProductDto>(items, totalCount, page, pageSize);
}

// ❌ Bad - Loading all data
public async Task<IEnumerable<Product>> GetAllProductsAsync()
{
    return await _context.Products.ToListAsync(); // Loads everything
}
```

#### Memory Management
```csharp
// ✅ Good - Dispose resources properly
public async Task<Stream> ExportProductsAsync()
{
    var memoryStream = new MemoryStream();
    
    try
    {
        // Export logic
        return memoryStream;
    }
    catch
    {
        await memoryStream.DisposeAsync();
        throw;
    }
}

// ✅ Good - Use using statements
public async Task ProcessFileAsync(string filePath)
{
    using var fileStream = new FileStream(filePath, FileMode.Open);
    // Process file
}
```

### Security Guidelines

#### Input Validation
```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommandRequest>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 200).WithMessage("Name must be between 1 and 200 characters")
            .Matches(@"^[a-zA-Z0-9\s\-_.]+$").WithMessage("Name contains invalid characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(1000000).WithMessage("Price cannot exceed 1,000,000");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .Must(BeValidDescription).WithMessage("Description contains prohibited content");
    }

    private bool BeValidDescription(string description)
    {
        if (string.IsNullOrEmpty(description)) return true;
        
        var prohibitedWords = new[] { "<script", "javascript:", "on[a-z]*=" };
        return !prohibitedWords.Any(word => description.Contains(word, StringComparison.OrdinalIgnoreCase));
    }
}
```

#### Authorization
```csharp
public class GetProductsEndpoint : EndpointBase<GetProductsRequest, GetProductsResponse>
{
    public override string? RolesAccess => "Admin,Manager"; // Role-based access
    
    // Or for anonymous access
    public override string? RolesAccess => null; // Anonymous access
}
```

## CI/CD Pipeline

### GitHub Actions Workflow
```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore --configuration Release
    
    - name: Test
      run: dotnet test --no-build --configuration Release --collect:"XPlat Code Coverage"
    
    - name: Code Coverage Report
      uses: codecov/codecov-action@v3
      with:
        files: ./coverage.cobertura.xml
        fail_ci_if_error: true

  build-and-deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Build Docker image
      run: docker build -t VisitorApp-api .
    
    - name: Deploy to staging
      run: |
        # Deployment commands
        echo "Deploying to staging environment"
```

### Quality Gates

#### Pre-commit Hooks
```bash
# Install husky
npm install --save-dev husky

# Setup pre-commit hook
npx husky add .husky/pre-commit "dotnet test"
npx husky add .husky/pre-commit "dotnet build"
```

#### Code Quality Metrics
- Code Coverage: حداقل 80%
- Maintainability Index: حداقل 70
- Cyclomatic Complexity: حداکثر 10 per method
- Code Duplication: حداکثر 5%

## Troubleshooting

### مشکلات رایج

#### 1. Migration Issues
```bash
# Error: Migration already exists
dotnet ef migrations remove --project VisitorApp.Persistence

# Error: Database connection
# Check connection string in appsettings.Development.json

# Error: Pending migrations
dotnet ef database update --project VisitorApp.Persistence --startup-project VisitorApp.API
```

#### 2. Build Errors
```bash
# Clear build artifacts
dotnet clean
dotnet restore
dotnet build

# Update packages
dotnet list package --outdated
dotnet add package <PackageName> --version <Version>
```

#### 3. Test Failures
```bash
# Run specific test
dotnet test --filter "TestClassName"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Debug tests
dotnet test --collect:"XPlat Code Coverage" --logger trx
```

### Performance Issues

#### Memory Leaks
```csharp
// Use memory profilers
// - dotMemory (JetBrains)
// - PerfView (Microsoft)
// - Visual Studio Diagnostic Tools

// Monitor memory usage
private readonly ILogger<ProductService> _logger;

public async Task ProcessLargeDataset()
{
    var initialMemory = GC.GetTotalMemory(false);
    
    try
    {
        // Process data
    }
    finally
    {
        var finalMemory = GC.GetTotalMemory(true);
        _logger.LogInformation("Memory used: {MemoryDelta} bytes", finalMemory - initialMemory);
    }
}
```

#### Database Performance
```sql
-- Enable Query Store
ALTER DATABASE VisitorApp SET QUERY_STORE = ON;

-- Monitor slow queries
SELECT 
    qt.query_sql_text,
    rs.avg_duration,
    rs.avg_cpu_time,
    rs.execution_count
FROM sys.query_store_query_text qt
JOIN sys.query_store_query q ON qt.query_text_id = q.query_text_id
JOIN sys.query_store_plan p ON q.query_id = p.query_id
JOIN sys.query_store_runtime_stats rs ON p.plan_id = rs.plan_id
ORDER BY rs.avg_duration DESC;
```

## Documentation Standards

### API Documentation
```csharp
/// <summary>
/// Creates a new product in the system
/// </summary>
/// <param name="request">Product creation request containing name, price, and description</param>
/// <param name="cancellationToken">Cancellation token for the async operation</param>
/// <returns>Created product with generated ID and creation timestamp</returns>
/// <response code="201">Product created successfully</response>
/// <response code="400">Invalid input data</response>
/// <response code="401">Unauthorized access</response>
/// <exception cref="ValidationException">Thrown when request validation fails</exception>
public async Task<CreateProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
{
    // Implementation
}
```

### README Updates
هر feature جدید باید documentation زیر را بروزرسانی کند:
- API endpoints در Postman collection
- Database schema changes
- Configuration changes
- Deployment notes

## Release Process

### Versioning Strategy
```
Major.Minor.Patch-PreRelease
1.2.3-alpha.1
1.2.3-beta.2
1.2.3 (stable)
```

### Release Checklist
- [ ] All tests passing
- [ ] Code review completed
- [ ] Documentation updated
- [ ] Migration scripts prepared
- [ ] Configuration changes documented
- [ ] Performance testing completed
- [ ] Security scan completed
- [ ] Backup strategy prepared

### Deployment Strategy
1. **Staging Deployment**: Test in staging environment
2. **Blue-Green Deployment**: Zero-downtime production deployment
3. **Database Migrations**: Run migrations during maintenance window
4. **Rollback Plan**: Prepared rollback procedures
5. **Monitoring**: Post-deployment monitoring and alerting 