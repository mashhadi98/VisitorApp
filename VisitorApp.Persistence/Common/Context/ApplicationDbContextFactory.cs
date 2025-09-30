using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VisitorApp.Persistence.Common.Context;

/// <summary>
/// Factory for creating ApplicationDbContext instances at design time (migrations, etc.)
/// This allows EF tools to create the context without running the full application
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Connection string for migrations
        // Note: This is only used during design-time (migrations)
        var connectionString = "Server=46.4.37.226\\MSSQLSERVER2022,51022;Database=FVisitorAppDb;User Id=FVisitorAppDb;Password=IW8J1et5j_jmnro*;TrustServerCertificate=True;";
        
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
