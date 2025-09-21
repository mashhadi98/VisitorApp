using VisitorApp.Persistence.Common.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VisitorApp.API;

/// <summary>
/// Factory for creating ApplicationDbContext instances at design time (migrations, etc.)
/// This allows EF tools to create the context without running the full application
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Get connection string from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
            
        var connectionString = configuration.GetConnectionString("ApplicationDb");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Default SQL Server connection string with Windows Authentication
            connectionString = "Server=localhost,11746;Database=VisitorAppDb;Trusted_Connection=True;TrustServerCertificate=True;";
        }
        
        var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
        
        optionsBuilder.UseSqlServer(connectionString,
            options => 
            {
                options.MigrationsAssembly(migrationsAssembly);
                options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
} 