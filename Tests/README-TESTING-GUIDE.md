# راهنمای جامع تست‌نویسی برای VisitorApp

## 📋 فهرست مطالب
1. [معماری تست](#معماری-تست)
2. [پروژه‌های تست](#پروژه‌های-تست)
3. [تست Domain Layer](#تست-domain-layer)
4. [تست Application Layer](#تست-application-layer)
5. [تست API Layer](#تست-api-layer)
6. [تست Integration](#تست-integration)
7. [تست ViewModels](#تست-viewmodels)
8. [بهترین روش‌ها](#بهترین-روشها)

---

## 🏗️ معماری تست

### ساختار پروژه‌های تست
```
VisitorApp.Tests/
├── VisitorApp.Domain.Tests/          # تست‌های Domain (Entities, Value Objects)
├── VisitorApp.Application.Tests/     # تست‌های Handlers و Validators
├── VisitorApp.API.Tests/              # تست‌های Integration
└── VisitorApp.Web.Tests/              # تست‌های ViewModels
```

### ابزارهای مورد نیاز
- **xUnit**: Test Framework
- **FluentAssertions**: Assertion Library
- **Moq**: Mocking Framework
- **AutoFixture**: Test Data Generator
- **Bogus**: Fake Data Generator
- **TestContainers**: برای تست‌های Integration با Database واقعی

---

## 📦 1. ایجاد پروژه‌های تست

### کامندهای ایجاد پروژه:
```bash
# Domain Tests
dotnet new xunit -n VisitorApp.Domain.Tests -o VisitorApp.Api/Tests/VisitorApp.Domain.Tests

# Application Tests  
dotnet new xunit -n VisitorApp.Application.Tests -o VisitorApp.Api/Tests/VisitorApp.Application.Tests

# API Integration Tests
dotnet new xunit -n VisitorApp.API.Tests -o VisitorApp.Api/Tests/VisitorApp.API.Tests

# Web ViewModels Tests
dotnet new xunit -n VisitorApp.Web.Tests -o VisitorApp.Web/Tests/VisitorApp.Web.Tests

# اضافه کردن به Solution
dotnet sln VisitorApp.Api/VisitorApp.Api.sln add VisitorApp.Api/Tests/**/*.csproj
dotnet sln VisitorApp.Web/VisitorApp.Web.sln add VisitorApp.Web/Tests/**/*.csproj
```

### نصب Packages:
```bash
# برای همه پروژه‌های تست
dotnet add package FluentAssertions
dotnet add package Moq
dotnet add package AutoFixture
dotnet add package AutoFixture.Xunit2
dotnet add package Bogus

# فقط برای Integration Tests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Testcontainers
dotnet add package Testcontainers.MsSql
```

---

## 🧪 2. تست Domain Layer

### مثال: تست Order Entity

**فایل**: `VisitorApp.Domain.Tests/Features/Orders/OrderTests.cs`

```csharp
public class OrderTests
{
    [Fact]
    public void Create_ValidData_ShouldCreateOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        // Act
        var order = Order.Create(customerId, userId);
        
        // Assert
        order.Should().NotBeNull();
        order.CustomerId.Should().Be(customerId);
        order.UserId.Should().Be(userId);
        order.Status.Should().Be(OrderStatus.Pending);
        order.OrderNumber.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void AddItem_ValidProduct_ShouldAddToOrder()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        var productId = Guid.NewGuid();
        var quantity = 2;
        var unitPrice = 100m;
        
        // Act
        order.AddItem(productId, quantity, unitPrice);
        
        // Assert
        order.Items.Should().HaveCount(1);
        order.TotalAmount.Should().Be(200m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AddItem_InvalidQuantity_ShouldThrowException(int quantity)
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        
        // Act & Assert
        Action act = () => order.AddItem(Guid.NewGuid(), quantity, 100m);
        act.Should().Throw<DomainException>()
            .WithMessage("*quantity*");
    }
    
    [Fact]
    public void Confirm_PendingOrder_ShouldChangeStatusToConfirmed()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        order.AddItem(Guid.NewGuid(), 1, 100m);
        
        // Act
        order.Confirm();
        
        // Assert
        order.Status.Should().Be(OrderStatus.Confirmed);
    }
    
    [Fact]
    public void Confirm_ConfirmedOrder_ShouldThrowException()
    {
        // Arrange
        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid());
        order.Confirm();
        
        // Act & Assert
        Action act = () => order.Confirm();
        act.Should().Throw<DomainException>();
    }
}
```

---

## 🔨 3. تست Application Layer

### مثال: تست CreateOrderCommandHandler

**فایل**: `VisitorApp.Application.Tests/Features/Orders/CreateOrderCommandHandlerTests.cs`

```csharp
public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IRepository<Product>> _productRepositoryMock;
    private readonly Mock<IRepository<Customer>> _customerRepositoryMock;
    private readonly CreateOrderCommandHandler _handler;
    
    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _productRepositoryMock = new Mock<IRepository<Product>>();
        _customerRepositoryMock = new Mock<IRepository<Customer>>();
        
        _handler = new CreateOrderCommandHandler(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _customerRepositoryMock.Object
        );
    }
    
    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        
        var customer = Customer.Create("Test Customer", "1234567890");
        var product = Product.Create("Test Product", "Model1", 100m, Guid.NewGuid());
        
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
            
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        
        var request = new CreateOrderCommandRequest
        {
            CustomerId = customerId,
            Items = new List<OrderItemRequest>
            {
                new() { ProductId = productId, Quantity = 2, UnitPrice = 100m }
            }
        };
        
        // Act
        var result = await _handler.Handler(request, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalAmount.Should().Be(200m);
        
        _orderRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), 
            Times.Once
        );
    }
    
    [Fact]
    public async Task Handle_CustomerNotFound_ShouldReturnFailure()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);
        
        var request = new CreateOrderCommandRequest
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemRequest>()
        };
        
        // Act
        var result = await _handler.Handler(request, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Customer");
    }
}
```

### مثال: تست Validator

**فایل**: `VisitorApp.Application.Tests/Features/Orders/CreateOrderValidatorTests.cs`

```csharp
public class CreateOrderValidatorTests
{
    private readonly CreateOrderCommandValidator _validator;
    
    public CreateOrderValidatorTests()
    {
        _validator = new CreateOrderCommandValidator();
    }
    
    [Fact]
    public void Validate_ValidRequest_ShouldPass()
    {
        // Arrange
        var request = new CreateOrderCommandRequest
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemRequest>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
            }
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void Validate_EmptyCustomerId_ShouldFail()
    {
        // Arrange
        var request = new CreateOrderCommandRequest
        {
            CustomerId = Guid.Empty,
            Items = new List<OrderItemRequest>()
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.CustomerId));
    }
    
    [Fact]
    public void Validate_EmptyItems_ShouldFail()
    {
        // Arrange
        var request = new CreateOrderCommandRequest
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemRequest>()
        };
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(request.Items));
    }
}
```

---

## 🌐 4. تست API Integration

### Setup WebApplicationFactory

**فایل**: `VisitorApp.API.Tests/Common/TestWebApplicationFactory.cs`

```csharp
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // حذف DbContext واقعی
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            
            if (descriptor != null)
                services.Remove(descriptor);
            
            // استفاده از InMemory Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });
            
            // Seed داده‌های تست
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            TestDataSeeder.SeedData(db);
        });
    }
}
```

### مثال: تست Order Endpoints

**فایل**: `VisitorApp.API.Tests/Features/Orders/OrdersEndpointTests.cs`

```csharp
public class OrdersEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;
    
    public OrdersEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetOrders_WithoutAuth_ShouldReturn401()
    {
        // Act
        var response = await _client.GetAsync("/api/orders");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task CreateOrder_ValidRequest_ShouldReturn200()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        
        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemRequest>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
            }
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<OrderDto>();
        result.Should().NotBeNull();
        result!.TotalAmount.Should().Be(100m);
    }
    
    [Fact]
    public async Task UpdateOrder_ValidRequest_ShouldUpdateOrder()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        
        var orderId = Guid.NewGuid(); // از داده‌های seed شده
        var request = new UpdateOrderRequest
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            Items = new List<OrderItemRequest>()
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/orders/{orderId}", request);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    private async Task<string> GetAuthTokenAsync()
    {
        var loginRequest = new { Username = "admin", Password = "Test@123" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result!.Token;
    }
}
```

---

## 📱 5. تست ViewModels (Web Layer)

### مثال: تست OrderListAppViewModel

**فایل**: `VisitorApp.Web.Tests/Features/Orders/OrderListAppViewModelTests.cs`

```csharp
public class OrderListAppViewModelTests
{
    private readonly Mock<IApiDispatcher> _dispatcherMock;
    private readonly Mock<INavigator> _navigatorMock;
    private readonly Mock<IToastService> _toastMock;
    private readonly Mock<IApiLogger> _loggerMock;
    private readonly Mock<IApiResultPolicy> _policyMock;
    private readonly OrderListAppViewModel _viewModel;
    
    public OrderListAppViewModelTests()
    {
        _dispatcherMock = new Mock<IApiDispatcher>();
        _navigatorMock = new Mock<INavigator>();
        _toastMock = new Mock<IToastService>();
        _loggerMock = new Mock<IApiLogger>();
        _policyMock = new Mock<IApiResultPolicy>();
        
        _viewModel = new OrderListAppViewModel(
            _dispatcherMock.Object,
            _navigatorMock.Object,
            _toastMock.Object,
            _loggerMock.Object,
            new ApiOptions { BaseUrl = "http://test" },
            _policyMock.Object
        );
    }
    
    [Fact]
    public async Task OnAppearingAsync_ShouldLoadOrders()
    {
        // Arrange
        var orders = new PagedResult<OrderDto>
        {
            Items = new List<OrderDto>
            {
                new() { Id = Guid.NewGuid(), OrderNumber = "ORD-001" }
            },
            TotalCount = 1,
            Page = 1,
            TotalPages = 1
        };
        
        _dispatcherMock
            .Setup(x => x.SendAsync<GetOrdersRequest, PagedResult<OrderDto>>(
                It.IsAny<GetOrdersRequest>(), null))
            .ReturnsAsync(Result<PagedResult<OrderDto>>.Success(orders));
        
        // Act
        await _viewModel.OnAppearingAsync();
        
        // Assert
        _viewModel.Items.Should().HaveCount(1);
        _viewModel.TotalCount.Should().Be(1);
    }
    
    [Fact]
    public async Task QuickFilterByStatusAsync_ShouldUpdateFilter()
    {
        // Arrange
        var status = 0; // Pending
        
        // Act
        await _viewModel.QuickFilterByStatusCommand.ExecuteAsync(status);
        
        // Assert
        _viewModel.Filter.Status.Should().Be(status);
        _dispatcherMock.Verify(
            x => x.SendAsync<GetOrdersRequest, PagedResult<OrderDto>>(
                It.Is<GetOrdersRequest>(r => r.Filter!.Status == status), 
                null),
            Times.Once
        );
    }
    
    [Fact]
    public void ShowDeleteConfirm_ValidId_ShouldOpenDialog()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _viewModel.Items = new List<OrderDto>
        {
            new() { Id = orderId, OrderNumber = "ORD-001" }
        };
        
        // Act
        _viewModel.ShowDeleteConfirmCommand.Execute(orderId);
        
        // Assert
        _viewModel.IsDeleteConfirmOpen.Should().BeTrue();
        _viewModel.OrderToDelete.Should().Be(orderId);
        _viewModel.OrderToDeleteNumber.Should().Be("ORD-001");
    }
}
```

---

## 📝 6. بهترین روش‌ها (Best Practices)

### Naming Conventions
```
[MethodName]_[Scenario]_[ExpectedResult]

مثال‌ها:
- Create_ValidData_ShouldCreateOrder
- Handle_CustomerNotFound_ShouldReturnFailure
- Validate_EmptyItems_ShouldFail
```

### Test Structure (AAA Pattern)
```csharp
[Fact]
public void TestMethod()
{
    // Arrange: آماده‌سازی
    var sut = new SystemUnderTest();
    var input = "test";
    
    // Act: اجرا
    var result = sut.Method(input);
    
    // Assert: بررسی نتیجه
    result.Should().Be(expected);
}
```

### Test Data Builders
```csharp
public class OrderBuilder
{
    private Guid _customerId = Guid.NewGuid();
    private Guid _userId = Guid.NewGuid();
    private List<OrderItem> _items = new();
    
    public OrderBuilder WithCustomer(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }
    
    public OrderBuilder WithItem(Guid productId, int qty, decimal price)
    {
        _items.Add(OrderItem.Create(productId, qty, price));
        return this;
    }
    
    public Order Build()
    {
        var order = Order.Create(_customerId, _userId);
        foreach (var item in _items)
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        return order;
    }
}

// Usage
var order = new OrderBuilder()
    .WithCustomer(customerId)
    .WithItem(productId, 2, 100m)
    .Build();
```

---

## 🚀 اجرای تست‌ها

### Command Line
```bash
# اجرای همه تست‌ها
dotnet test

# اجرای تست‌های یک پروژه خاص
dotnet test VisitorApp.Api/Tests/VisitorApp.Domain.Tests/

# اجرا با Coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# فیلتر تست‌ها
dotnet test --filter "FullyQualifiedName~Orders"
```

### Visual Studio
- Test Explorer: View → Test Explorer
- Run All Tests: Ctrl+R, A
- Debug Test: راست کلیک → Debug

---

## 📊 Code Coverage

### نصب ابزار
```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

### تولید گزارش
```bash
# جمع‌آوری Coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# تولید HTML Report
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report

# باز کردن گزارش
start coverage-report/index.html
```

### اهداف Coverage
- **Domain Layer**: 90%+
- **Application Layer**: 80%+
- **API Layer**: 70%+
- **Integration Tests**: 60%+

---

## 🔄 CI/CD Integration

### GitHub Actions (.github/workflows/tests.yml)
```yaml
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal /p:CollectCoverage=true
    
    - name: Upload Coverage
      uses: codecov/codecov-action@v3
```

---

## 📚 منابع بیشتر

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions](https://fluentassertions.com/)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)
- [Test Containers](https://dotnet.testcontainers.org/)

---

**نکته**: این راهنما یک چارچوب کامل برای تست‌نویسی است. برای پوشش کامل سیستم، باید تست‌های مشابه برای تمام Features نوشته شود.



