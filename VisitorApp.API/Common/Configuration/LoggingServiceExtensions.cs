using VisitorApp.Persistence.Common.Context;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;

namespace VisitorApp.API.Common.Configuration;

/// <summary>
/// Extension methods for configuring logging services
/// </summary>
public static class LoggingServiceExtensions
{
    /// <summary>
    /// Configures Serilog logging services with console and MySQL database logging
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddLoggingServices(this IServiceCollection services)
    {
        // Get connection string from environment variable
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ApplicationDb");
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.MariaDB(
                connectionString: connectionString,
                tableName: "Logs",
                autoCreateTable: true,
                options: new MariaDBSinkOptions())
            .Enrich.FromLogContext()
            .CreateLogger();

        return services;
    }
}