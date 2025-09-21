using VisitorApp.Application.Common.Interfaces;

namespace VisitorApp.API;

/// <summary>
/// کلاس تستی برای بررسی صحت کارکرد FileStorageService در زمان راه‌اندازی
/// </summary>
public static class TestFileStorageStartup
{
    /// <summary>
    /// تست اولیه سرویس مدیریت فایل برای اطمینان از عدم وجود خطای WebRootPath
    /// </summary>
    /// <param name="app">نمونه WebApplication</param>
    public static async Task TestFileStorageService(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var fileStorageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            // تست تولید نام فایل منحصر به فرد
            var uniqueFileName = fileStorageService.GenerateUniqueFileName("test-image.jpg", "images");
            logger.LogInformation("✅ File Storage Service initialized successfully. Test unique filename: {FileName}", uniqueFileName);
            
            // تست دریافت URL فایل
            var testUrl = fileStorageService.GetFileUrl("uploads/test.jpg");
            logger.LogInformation("✅ File URL generation test successful: {Url}", testUrl);
            
            logger.LogInformation("🎉 File Storage Service is ready to use!");
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "❌ File Storage Service test failed: {Error}", ex.Message);
            throw;
        }
    }
} 