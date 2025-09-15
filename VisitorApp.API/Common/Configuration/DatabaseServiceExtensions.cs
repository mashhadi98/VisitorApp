using VisitorApp.Persistence.Common.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace VisitorApp.API.Common.Configuration;

/// <summary>
/// Extension methods for configuring MySQL database services
/// </summary>
public static class DatabaseServiceExtensions
{
	/// <summary>
	/// Configures MySQL database context using environment variable for connection string
	/// </summary>
	/// <param name="services">The service collection</param>
	/// <param name="configuration">The configuration instance</param>
	/// <returns>The service collection for chaining</returns>
	public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
	{
		// Get connection string from configuration or environment variable
		var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ApplicationDb")
			?? configuration.GetConnectionString("ApplicationDb");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new InvalidOperationException("ApplicationDb connection string is not configured. Please set it in appsettings.json or environment variables.");
		}

		var migrationsAssembly = VisitorApp.Persistence.AssemblyReference.Assembly.GetName().Name;

		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseMySql(connectionString,
				ServerVersion.Create(new Version(8, 0, 21), ServerType.MySql),
				op => op.MigrationsAssembly(migrationsAssembly));
		});

		return services;
	}
	public static async Task<IApplicationBuilder> AddDatabaseApplication(this IApplicationBuilder app, IConfiguration configuration)
	{
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;
            var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();

            await applicationDbContext.Database.MigrateAsync();
        }
        return app;
	}

	/// <summary>
	/// Checks if MySQL provider is configured by detecting MySQL-specific connection string patterns
	/// </summary>
	/// <param name="configuration">The configuration instance</param>
	/// <returns>True if MySQL provider should be used</returns>
	public static bool IsMySqlConfigured(IConfiguration configuration)
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

		// Check for MySQL-specific keywords in connection string
		var lowerConnectionString = connectionString.ToLowerInvariant();
		return lowerConnectionString.Contains("server=") || 
			   lowerConnectionString.Contains("data source=") ||
			   lowerConnectionString.Contains("port=3306") ||
			   lowerConnectionString.Contains("mysql");
	}
} 