using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Domain.Features.Customers.Entities;
using VisitorApp.Domain.Features.Orders.Entities;
using VisitorApp.Persistence.Common.Context;
using Xunit;

namespace VisitorApp.Application.Tests.Orders;

public class SimpleOrderCreationTest : IDisposable
{
    private readonly ApplicationDbContext _context;
    
    public SimpleOrderCreationTest()
    {
        // Direct connection string - same as API
        var connectionString = "Server=46.4.37.226\\MSSQLSERVER2022,51022;Database=FVisitorAppDb;User Id=FVisitorAppDb;Password=IW8J1et5j_jmnro*;TrustServerCertificate=True;";
        
        // Setup real database connection
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine)
            .Options;
        
        _context = new ApplicationDbContext(options);
        
        Console.WriteLine($"=== Connected to database ===");
    }
    
    [Fact]
    public async Task DirectOrderCreation_ShouldRevealActualError()
    {
        // Using EXACT data from user's request
        var customerId = Guid.Parse("a4c5993e-30ca-461f-9078-288fb0a0b43f");
        var productId = Guid.Parse("746ccd2a-37c0-42aa-9ec6-bc51faf8cf56");
        
        Console.WriteLine($"\n=== Checking Database Entities ===");
        Console.WriteLine($"CustomerId: {customerId}");
        Console.WriteLine($"ProductId: {productId}");
        
        try
        {
            // Check if customer exists
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                Console.WriteLine("❌ Customer NOT found in database!");
                throw new Exception($"Customer {customerId} does not exist. Please create it first.");
            }
            Console.WriteLine($"✅ Customer found: {customer.FullName}");
            
            // Check if product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine("❌ Product NOT found in database!");
                throw new Exception($"Product {productId} does not exist. Please create it first.");
            }
            Console.WriteLine($"✅ Product found: {product.Title}, Price: {product.Price}");
            
            Console.WriteLine($"\n=== Creating Order ===");
            
            // Create order entity
            var order = new Order
            {
                OrderNumber = $"#TEST{DateTime.Now:HHmmss}",
                CustomerId = customerId,
                UserId = null,  // No user for this test
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = 0
            };
            
            Console.WriteLine($"Order Number: {order.OrderNumber}");
            Console.WriteLine($"Order ID (before save): {order.Id}");
            
            // Create order item - WITHOUT setting OrderId explicitly
            var orderItem = new OrderItem
            {
                // NOT setting OrderId here - let EF Core handle it
                ProductId = productId,
                Quantity = 1,
                UnitPrice = 100000m
            };
            
            Console.WriteLine($"OrderItem ID (before save): {orderItem.Id}");
            Console.WriteLine($"OrderItem.OrderId (before adding to collection): {orderItem.OrderId}");
            
            // Add item to order collection
            order.Items.Add(orderItem);
            order.TotalAmount = orderItem.TotalPrice;
            
            Console.WriteLine($"OrderItem.OrderId (after adding to collection): {orderItem.OrderId}");
            
            // Add order to context
            _context.Orders.Add(order);
            
            Console.WriteLine($"\n=== Attempting SaveChanges ===");
            
            // This is where the error should happen
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"\n=== SUCCESS! ===");
            Console.WriteLine($"Order ID (after save): {order.Id}");
            Console.WriteLine($"OrderItem ID (after save): {orderItem.Id}");
            Console.WriteLine($"OrderItem.OrderId (after save): {orderItem.OrderId}");
            
            // Cleanup - delete the test order
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Test order cleaned up.");
            
            Assert.True(true, "Order created successfully!");
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"\n=== ❌ DATABASE UPDATE EXCEPTION ===");
            Console.WriteLine($"Message: {dbEx.Message}");
            
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"\n--- Inner Exception ---");
                Console.WriteLine($"Type: {dbEx.InnerException.GetType().Name}");
                Console.WriteLine($"Message: {dbEx.InnerException.Message}");
                
                if (dbEx.InnerException.InnerException != null)
                {
                    Console.WriteLine($"\n--- Inner Inner Exception ---");
                    Console.WriteLine($"Type: {dbEx.InnerException.InnerException.GetType().Name}");
                    Console.WriteLine($"Message: {dbEx.InnerException.InnerException.Message}");
                }
            }
            
            Console.WriteLine($"\n--- Stack Trace ---");
            Console.WriteLine(dbEx.StackTrace);
            
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n=== ❌ GENERAL EXCEPTION ===");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            
            if (ex.InnerException != null)
            {
                Console.WriteLine($"\nInner Exception: {ex.InnerException.Message}");
            }
            
            Console.WriteLine($"\n--- Stack Trace ---");
            Console.WriteLine(ex.StackTrace);
            
            throw;
        }
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}

