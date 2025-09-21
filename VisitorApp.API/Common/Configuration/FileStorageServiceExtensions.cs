using VisitorApp.Application.Common.Interfaces;
using VisitorApp.Infrastructure.Common.Services;

namespace VisitorApp.API.Common.Configuration;

/// <summary>
/// Extension methods for configuring file storage services
/// </summary>
public static class FileStorageServiceExtensions
{
    /// <summary>
    /// Adds file storage services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddFileStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register file storage service
        services.AddScoped<IFileStorageService, FileStorageService>();

        // Configure static files serving
        services.Configure<StaticFileOptions>(options =>
        {
            var uploadsPath = configuration["FileStorage:UploadsPath"] ?? "uploads";
            options.RequestPath = $"/{uploadsPath}";
        });

        return services;
    }

    /// <summary>
    /// Configures static files middleware for serving uploaded files
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseFileStorage(this IApplicationBuilder app, IConfiguration configuration)
    {
        var uploadsPath = configuration["FileStorage:UploadsPath"] ?? "uploads";
        var webHostEnvironment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        
        // Handle null WebRootPath - set default if not configured
        var webRoot = webHostEnvironment.WebRootPath;
        if (string.IsNullOrEmpty(webRoot))
        {
            // Set default wwwroot path relative to ContentRootPath
            webRoot = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");
            
            // Create wwwroot directory if it doesn't exist
            if (!Directory.Exists(webRoot))
            {
                Directory.CreateDirectory(webRoot);
                
                // Update WebRootPath for other middleware
                var webRootProperty = typeof(IWebHostEnvironment).GetProperty(nameof(IWebHostEnvironment.WebRootPath));
                if (webRootProperty?.CanWrite == true)
                {
                    webRootProperty.SetValue(webHostEnvironment, webRoot);
                }
            }
        }

        var fullUploadsPath = Path.Combine(webRoot, uploadsPath);

        // Create uploads directory if it doesn't exist
        if (!Directory.Exists(fullUploadsPath))
        {
            Directory.CreateDirectory(fullUploadsPath);
        }

        // Serve static files from uploads directory
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(fullUploadsPath),
            RequestPath = $"/{uploadsPath}",
            ServeUnknownFileTypes = false,
            OnPrepareResponse = context =>
            {
                // Add security headers
                context.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Context.Response.Headers.Append("X-Frame-Options", "DENY");
                
                // Set cache headers for images
                if (context.File.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    context.File.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    context.File.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    context.File.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                    context.File.Name.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                {
                    context.Context.Response.Headers.Append("Cache-Control", "public, max-age=86400"); // 24 hours
                }
            }
        });

        return app;
    }
} 