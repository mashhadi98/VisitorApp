// Quick and dirty test - compile and run this standalone
// dotnet build && dotnet run --project QuickOrderTest.csproj

using Microsoft.EntityFrameworkCore;
using VisitorApp.Domain.Features.Orders.Entities;
using VisitorApp.Persistence.Common.Context;

Console.WriteLine("=== Quick Order Test ===\n");

var connectionString = "Server=46.4.37.226\\MSSQLSERVER2022,51022;Database=FVisitorAppDb;User Id=FVisitorAppDb;Password=IW8J1et5j_jmnro*;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;

using var context = new ApplicationDbContext(options);

// Using EXACT data from user's request
var customerId = Guid.Parse("a4c5993e-30ca-461f-9078-288fb0a0b43f");
var productId = Guid.Parse("746ccd2a-37c0-42aa-9ec6-bc51faf8cf56");

Console.WriteLine($"CustomerId: {customerId}");
Console.WriteLine($"ProductId: {productId}\n");

// Check entities exist
var customerExists = await context.Customers.AnyAsync(c => c.Id == customerId);
var productExists = await context.Products.AnyAsync(p => p.Id == productId);

Console.WriteLine($"Customer exists: {customerExists}");
Console.WriteLine($"Product exists: {productExists}\n");

if (!customerExists || !productExists)
{
    Console.WriteLine("ERROR: Customer or Product does not exist!");
    return;
}

try
{
    Console.WriteLine("Creating order...");
    
    var order = new Order
    {
        OrderNumber = $"#TEST{DateTime.Now:HHmmss}",
        CustomerId = customerId,
        UserId = null,
        OrderDate = DateTime.UtcNow,
        Status = OrderStatus.Pending,
        TotalAmount = 0
    };
    
    var orderItem = new OrderItem
    {
        // NOT setting OrderId - let EF handle it
        ProductId = productId,
        Quantity = 1,
        UnitPrice = 100000m
    };
    
    order.Items.Add(orderItem);
    order.TotalAmount = 100000m;
    
    context.Orders.Add(order);
    
    Console.WriteLine("Saving to database...");
    await context.SaveChangesAsync();
    
    Console.WriteLine($"\n✅ SUCCESS! Order ID: {order.Id}");
    Console.WriteLine($"Order Number: {order.OrderNumber}");
    Console.WriteLine($"OrderItem ID: {orderItem.Id}");
    
    // Cleanup
    context.Orders.Remove(order);
    await context.SaveChangesAsync();
    Console.WriteLine("Test order cleaned up.");
}
catch (DbUpdateException ex)
{
    Console.WriteLine($"\n❌ DATABASE UPDATE ERROR:");
    Console.WriteLine($"Message: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"\nInner Exception: {ex.InnerException.Message}");
        if (ex.InnerException.InnerException != null)
        {
            Console.WriteLine($"\nInner Inner Exception: {ex.InnerException.InnerException.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ ERROR: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
}

