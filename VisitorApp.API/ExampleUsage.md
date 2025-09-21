# 📁 راهنمای استفاده از سرویس مدیریت فایل

این سند نحوه استفاده از سیستم مدیریت فایل و تصاویر در پروژه را نشان می‌دهد.

## 🔧 تنظیمات اولیه

### 1. تنظیمات appsettings.json
```json
{
  "FileStorage": {
    "UploadsPath": "uploads",
    "BaseUrl": "https://localhost:7101",
    "MaxFileSize": 10485760,
    "AllowedImageExtensions": [ ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" ],
    "AllowedDocumentExtensions": [ ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".rtf", ".csv" ],
    "EnableImageProcessing": true,
    "DefaultImageQuality": 85,
    "DefaultThumbnailSize": 150
  }
}
```

### 2. ثبت سرویس‌ها در Program.cs
```csharp
// اضافه شده است - نیازی به تغییر نیست
builder.Services.AddFileStorageServices(builder.Configuration);
app.UseFileStorage(builder.Configuration);
```

## 🚀 نحوه استفاده در لایه Application

### 1. تزریق سرویس در کنترلر یا Handler
```csharp
public class MyHandler
{
    private readonly IFileStorageService _fileStorageService;
    
    public MyHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }
}
```

### 2. آپلود ساده تصویر
```csharp
public async Task<FileUploadResult> UploadSimpleImage(IFormFile imageFile)
{
    var request = new FileUploadRequest
    {
        File = imageFile,
        Folder = "images",
        ExpectedFileType = FileType.Image,
        GenerateUniqueFileName = true
    };

    return await _fileStorageService.UploadFileAsync(request);
}
```

### 3. آپلود تصویر با پردازش
```csharp
public async Task<FileUploadResult> UploadProcessedImage(IFormFile imageFile)
{
    var request = new FileUploadRequest
    {
        File = imageFile,
        Folder = "avatars",
        ExpectedFileType = FileType.Image,
        GenerateUniqueFileName = true,
        
        // تنظیمات پردازش تصویر
        ShouldResize = true,
        ResizeWidth = 800,
        ResizeHeight = 600,
        ImageQuality = 90,
        
        // ایجاد thumbnail
        CreateThumbnail = true,
        ThumbnailSize = 200,
        
        // محدودیت‌های امنیتی
        MaxFileSize = 5 * 1024 * 1024, // 5MB
        AllowedExtensions = new[] { ".jpg", ".jpeg", ".png" }
    };

    return await _fileStorageService.UploadFileAsync(request);
}
```

### 4. آپلود سند
```csharp
public async Task<FileUploadResult> UploadDocument(IFormFile documentFile)
{
    var request = new FileUploadRequest
    {
        File = documentFile,
        Folder = "documents",
        ExpectedFileType = FileType.Document,
        MaxFileSize = 10 * 1024 * 1024, // 10MB
        AllowedExtensions = new[] { ".pdf", ".doc", ".docx" }
    };

    return await _fileStorageService.UploadFileAsync(request);
}
```

### 5. آپلود چندین فایل
```csharp
public async Task<List<FileUploadResult>> UploadMultipleFiles(List<IFormFile> files)
{
    var requests = files.Select(file => new FileUploadRequest
    {
        File = file,
        Folder = "gallery",
        GenerateUniqueFileName = true
    }).ToList();

    return await _fileStorageService.UploadFilesAsync(requests);
}
```

### 6. حذف فایل
```csharp
public async Task<bool> DeleteFile(string filePath)
{
    return await _fileStorageService.DeleteFileAsync(filePath);
}
```

### 7. بررسی وجود فایل
```csharp
public async Task<bool> CheckFileExists(string filePath)
{
    return await _fileStorageService.FileExistsAsync(filePath);
}
```

### 8. دریافت اطلاعات فایل
```csharp
public async Task<FileInfo?> GetFileInfo(string filePath)
{
    return await _fileStorageService.GetFileInfoAsync(filePath);
}
```

### 9. دریافت URL فایل
```csharp
public string GetFileUrl(string filePath)
{
    return _fileStorageService.GetFileUrl(filePath);
}
```

## 🌐 API Endpoints

### آپلود فایل ساده
```http
POST /api/Files/upload
Content-Type: multipart/form-data

{
  "File": [فایل],
  "Folder": "images",
  "ExpectedFileType": "Image"
}
```

### آپلود تصویر با پردازش
```http
POST /api/Files/upload-image
Content-Type: multipart/form-data

{
  "file": [فایل تصویر],
  "folder": "avatars",
  "width": 800,
  "height": 600,
  "quality": 90,
  "createThumbnail": true,
  "thumbnailSize": 200
}
```

### آپلود چندین فایل
```http
POST /api/Files/upload-multiple
Content-Type: multipart/form-data

{
  "files": [آرایه فایل‌ها],
  "folder": "gallery",
  "generateUniqueFileName": true
}
```

### حذف فایل
```http
DELETE /api/Files?filePath=uploads/images/sample.jpg
```

### بررسی وجود فایل
```http
GET /api/Files/exists?filePath=uploads/images/sample.jpg
```

### دریافت اطلاعات فایل
```http
GET /api/Files/info?filePath=uploads/images/sample.jpg
```

## 📝 نمونه پاسخ‌ها

### پاسخ موفق آپلود
```json
{
  "isSuccess": true,
  "filePath": "uploads/images/avatar_20231201_143022_a1b2c3d4.jpg",
  "fileName": "avatar_20231201_143022_a1b2c3d4.jpg",
  "fileUrl": "https://localhost:7101/uploads/images/avatar_20231201_143022_a1b2c3d4.jpg",
  "fileSize": 156789,
  "contentType": "image/jpeg",
  "fileExtension": ".jpg",
  "thumbnailPath": "uploads/images/thumbnails/avatar_20231201_143022_a1b2c3d4_thumb.jpg",
  "thumbnailUrl": "https://localhost:7101/uploads/images/thumbnails/avatar_20231201_143022_a1b2c3d4_thumb.jpg",
  "uploadedAt": "2023-12-01T14:30:22.123Z"
}
```

### پاسخ خطا در آپلود
```json
{
  "isSuccess": false,
  "errorMessage": "فایل نامعتبر است",
  "errorDetails": [
    "سایز فایل نمی‌تواند بیشتر از 5.0 MB باشد",
    "نوع فایل مجاز نیست. انواع مجاز: .jpg, .jpeg, .png"
  ]
}
```

## 🔒 امنیت و اعتبارسنجی

### محدودیت‌های پیش‌فرض
- **حداکثر سایز فایل:** 10MB
- **انواع تصویر مجاز:** jpg, jpeg, png, gif, bmp, webp, svg
- **انواع سند مجاز:** pdf, doc, docx, xls, xlsx, txt, rtf, csv
- **بررسی محتوا:** کنترل فایل‌های مخرب
- **نام‌گذاری منحصر به فرد:** جلوگیری از تداخل فایل‌ها

### تنظیمات امنیتی
```csharp
var request = new FileUploadRequest
{
    File = file,
    
    // محدودیت سایز (بایت)
    MaxFileSize = 5 * 1024 * 1024, // 5MB
    
    // فقط انواع خاص
    AllowedExtensions = new[] { ".jpg", ".png" },
    
    // بررسی نوع فایل
    ExpectedFileType = FileType.Image
};
```

## 📂 ساختار پوشه‌ها

```
wwwroot/
├── uploads/
│   ├── images/           # تصاویر عمومی
│   ├── avatars/          # تصاویر پروفایل
│   ├── documents/        # اسناد
│   ├── gallery/          # گالری تصاویر
│   └── thumbnails/       # تصاویر کوچک
```

## ⚡ نکات مهم

1. **تولید نام منحصر به فرد:** همیشه فعال کنید تا فایل‌ها جایگزین نشوند
2. **استفاده از thumbnail:** برای تصاویر بزرگ حتماً thumbnail ایجاد کنید
3. **محدود کردن سایز:** همیشه حداکثر سایز مجاز تعیین کنید
4. **بررسی نوع فایل:** فقط انواع مورد نیاز را مجاز کنید
5. **مدیریت خطا:** همیشه نتیجه آپلود را بررسی کنید

## 🎯 نمونه کاربرد در محصولات

```csharp
// اضافه کردن تصویر به محصول
public async Task<string?> AddProductImage(Guid productId, IFormFile imageFile)
{
    var request = new FileUploadRequest
    {
        File = imageFile,
        Folder = $"products/{productId}",
        ExpectedFileType = FileType.Image,
        ShouldResize = true,
        ResizeWidth = 800,
        ResizeHeight = 600,
        CreateThumbnail = true,
        ThumbnailSize = 200
    };

    var result = await _fileStorageService.UploadFileAsync(request);
    
    if (result.IsSuccess)
    {
        // ذخیره مسیر در دیتابیس
        await UpdateProductImage(productId, result.FilePath!);
        return result.FileUrl;
    }

    return null;
}
``` 