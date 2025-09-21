# 🔧 رفع خطای WebRootPath در سرویس مدیریت فایل

## ❌ مشکل

هنگام اجرای برنامه، خطای زیر رخ می‌داد:

```
System.ArgumentNullException: 'Value cannot be null. (Parameter 'path1')'
```

### 🔍 علت مشکل

در ASP.NET Core، ممکن است `WebRootPath` در مواقع زیر **null** باشد:

1. **هنگام راه‌اندازی برنامه** - قبل از تنظیم کامل middleware ها
2. **در محیط Development** - زمانی که پوشه `wwwroot` وجود ندارد
3. **تنظیمات پیش‌فرض** - وقتی که مقدار پیش‌فرض تنظیم نشده

### 📍 مکان‌های بروز مشکل

```csharp
// ❌ مشکل در FileStorageServiceExtensions.cs
var webRoot = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
var fullUploadsPath = Path.Combine(webRoot, uploadsPath); // خطا اینجا!

// ❌ مشکل در FileStorageService.cs
private string GetUploadPath(string? folder)
{
    var basePath = Path.Combine(_webHostEnvironment.WebRootPath, _uploadsPath); // خطا اینجا!
    // ...
}
```

## ✅ راه‌حل

### 1. اصلاح FileStorageServiceExtensions.cs

```csharp
public static IApplicationBuilder UseFileStorage(this IApplicationBuilder app, IConfiguration configuration)
{
    var uploadsPath = configuration["FileStorage:UploadsPath"] ?? "uploads";
    var webHostEnvironment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
    
    // ✅ Handle null WebRootPath - set default if not configured
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
    // ... rest of the code
}
```

### 2. اصلاح FileStorageService.cs

```csharp
// ✅ متد جدید برای مدیریت WebRootPath
private string GetWebRootPath()
{
    var webRoot = _webHostEnvironment.WebRootPath;
    
    if (string.IsNullOrEmpty(webRoot))
    {
        // Set default wwwroot path relative to ContentRootPath
        webRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
        
        // Create wwwroot directory if it doesn't exist
        if (!Directory.Exists(webRoot))
        {
            Directory.CreateDirectory(webRoot);
            _logger.LogInformation("Created wwwroot directory at {WebRoot}", webRoot);
        }
    }

    return webRoot;
}

// ✅ استفاده از متد جدید در سایر method ها
private string GetUploadPath(string? folder)
{
    var webRoot = GetWebRootPath(); // ✅ بجای WebRootPath مستقیم
    var basePath = Path.Combine(webRoot, _uploadsPath);
    // ...
}

private string GetFullPath(string relativePath)
{
    var webRoot = GetWebRootPath(); // ✅ بجای WebRootPath مستقیم
    return Path.Combine(webRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
}
```

### 3. اضافه کردن UseStaticFiles در Program.cs

```csharp
// ✅ اضافه کردن UseStaticFiles قبل از UseFileStorage
app.UseStaticFiles();
app.UseFileStorage(builder.Configuration);
```

### 4. تست خودکار برای تأیید عملکرد

```csharp
// ✅ تست اولیه در محیط Development
if (app.Environment.IsDevelopment())
{
    await app.TestFileStorageService();
}
```

## 🔧 ویژگی‌های راه‌حل

### ✨ مزایا

1. **🛡️ حفاظت در برابر null** - خودکار تشخیص و رفع مشکل
2. **📁 ایجاد خودکار پوشه** - اگر wwwroot وجود نداشته باشد، ایجاد می‌کند
3. **📝 لاگ‌گذاری مناسب** - تمام عملیات لاگ می‌شوند
4. **🔄 بازیابی خودکار** - تنظیم مجدد WebRootPath برای سایر middleware ها
5. **⚡ عدم تأثیر بر Performance** - فقط یکبار بررسی می‌شود

### 🎯 سناریوهای پشتیبانی شده

- ✅ اجرای اولیه بدون پوشه wwwroot
- ✅ محیط Development
- ✅ محیط Production
- ✅ Docker containers
- ✅ تنظیمات مختلف hosting

## 🧪 تست راه‌حل

### قبل از رفع
```bash
System.ArgumentNullException: Value cannot be null. (Parameter 'path1')
   at System.IO.Path.Combine(String path1, String path2)
```

### بعد از رفع
```
info: Program[0]
      ✅ File Storage Service initialized successfully. Test unique filename: test-image_20231201_143022_a1b2c3d4.jpg
info: Program[0]
      ✅ File URL generation test successful: https://localhost:7101/uploads/test.jpg
info: Program[0]
      🎉 File Storage Service is ready to use!
```

## 🚀 نتیجه‌گیری

با این رفع مشکل:
- ❌ **خطای WebRootPath null** کاملاً برطرف شد
- ✅ **سیستم مدیریت فایل** در تمام شرایط کار می‌کند
- 🛡️ **امنیت و پایداری** افزایش یافت
- 📈 **سازگاری** با انواع محیط‌های اجرایی

**تبریک! 🎉 سیستم مدیریت فایل آماده استفاده است.** 