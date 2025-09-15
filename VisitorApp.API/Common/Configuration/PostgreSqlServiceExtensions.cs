using VisitorApp.Persistence.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace VisitorApp.API.Common.Configuration;

/// <summary>
/// Extension methods for configuring PostgreSQL database services
/// </summary>
public static class PostgreSqlServiceExtensions
{
    /// <summary>
    /// Configures PostgreSQL database context using environment variable for connection string
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration instance</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddPostgreSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Try environment variable first
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ApplicationDb");
        
        // Fallback to configuration
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = configuration.GetConnectionString("ApplicationDb");
        }
        
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("PostgreSQL connection string 'ConnectionStrings__ApplicationDb' environment variable is not configured");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("VisitorApp.Persistence"));
        });

        return services;
    }

    /// <summary>
    /// Checks if PostgreSQL provider is configured by detecting PostgreSQL-specific connection string patterns
    /// </summary>
    /// <param name="configuration">The configuration instance</param>
    /// <returns>True if PostgreSQL provider should be used</returns>
    public static bool IsPostgreSqlConfigured(IConfiguration configuration)
    {
        // Try environment variable first
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ApplicationDb");
        
        // Fallback to configuration
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = configuration.GetConnectionString("ApplicationDb");
        }
        
        if (string.IsNullOrEmpty(connectionString))
            return false;

        // Check for PostgreSQL-specific keywords in connection string
        var lowerConnectionString = connectionString.ToLowerInvariant();
        return lowerConnectionString.Contains("host=") ||
               lowerConnectionString.Contains("username=") ||
               lowerConnectionString.Contains("port=5432") ||
               lowerConnectionString.Contains("postgres");
    }
} 