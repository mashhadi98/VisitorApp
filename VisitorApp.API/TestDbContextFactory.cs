using VisitorApp.Persistence.Common.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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
            // Use the same server as configured in launchSettings.json for consistency
            connectionString = "Server=192.168.10.150;Port=11002;Database=VisitorAppDb;Uid=root;Pwd=Admin123!";
        }
        
        var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
        
        optionsBuilder.UseMySql(connectionString,
            ServerVersion.Create(8, 0, 21, ServerType.MySql),
            options => options.MigrationsAssembly(migrationsAssembly));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
} 