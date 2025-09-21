using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using VisitorApp.Application.Common.DTOs;
using VisitorApp.Application.Common.Interfaces;

namespace VisitorApp.Infrastructure.Common.Services;

/// <summary>
/// پیاده‌سازی سرویس مدیریت فایل‌ها
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FileStorageService> _logger;

    // تنظیمات پیش‌فرض
    private readonly string _uploadsPath;
    private readonly string _baseUrl;
    private readonly long _defaultMaxFileSize;
    private readonly string[] _defaultAllowedImageExtensions;
    private readonly string[] _defaultAllowedDocumentExtensions;

    public FileStorageService(
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration,
        ILogger<FileStorageService> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
        _logger = logger;

        // خواندن تنظیمات از appsettings
        _uploadsPath = _configuration["FileStorage:UploadsPath"] ?? "uploads";
        _baseUrl = _configuration["FileStorage:BaseUrl"] ?? "";
        _defaultMaxFileSize = long.Parse(_configuration["FileStorage:MaxFileSize"] ?? "10485760"); // 10MB
        _defaultAllowedImageExtensions = _configuration.GetSection("FileStorage:AllowedImageExtensions").Get<string[]>() 
            ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        _defaultAllowedDocumentExtensions = _configuration.GetSection("FileStorage:AllowedDocumentExtensions").Get<string[]>() 
            ?? new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".rtf" };
    }

    public async Task<FileUploadResult> UploadFileAsync(FileUploadRequest fileUploadRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            // اعتبارسنجی فایل
            var validationResult = await ValidateFileAsync(fileUploadRequest);
            if (!validationResult.IsValid)
            {
                return FileUploadResult.Failure("فایل نامعتبر است", validationResult.Errors);
            }

            // ایجاد مسیر آپلود
            var uploadPath = GetUploadPath(fileUploadRequest.Folder);
            Directory.CreateDirectory(uploadPath);

            // تولید نام فایل
            var fileName = fileUploadRequest.GenerateUniqueFileName
                ? GenerateUniqueFileName(fileUploadRequest.File.FileName, fileUploadRequest.Folder)
                : fileUploadRequest.CustomFileName ?? fileUploadRequest.File.FileName;

            var filePath = Path.Combine(uploadPath, fileName);
            var relativePath = Path.Combine(_uploadsPath, fileUploadRequest.Folder ?? "", fileName).Replace("\\", "/");

            // ذخیره فایل
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileUploadRequest.File.CopyToAsync(fileStream, cancellationToken);
            }

            _logger.LogInformation("فایل {FileName} با موفقیت آپلود شد", fileName);

            var result = FileUploadResult.Success(
                relativePath,
                fileName,
                GetFileUrl(relativePath),
                fileUploadRequest.File.Length,
                fileUploadRequest.File.ContentType
            );

            // پردازش تصویر (تغییر اندازه و thumbnail)
            if (IsImageFile(fileUploadRequest.File.ContentType))
            {
                await ProcessImageAsync(fileUploadRequest, filePath, result, cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در آپلود فایل {FileName}", fileUploadRequest.File.FileName);
            return FileUploadResult.Failure($"خطا در آپلود فایل: {ex.Message}");
        }
    }

    public async Task<List<FileUploadResult>> UploadFilesAsync(List<FileUploadRequest> fileUploadRequests, CancellationToken cancellationToken = default)
    {
        var results = new List<FileUploadResult>();

        foreach (var request in fileUploadRequests)
        {
            var result = await UploadFileAsync(request, cancellationToken);
            results.Add(result);
        }

        return results;
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = GetFullPath(filePath);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("فایل {FilePath} حذف شد", filePath);

                // حذف thumbnail در صورت وجود
                var thumbnailPath = GetThumbnailPath(filePath);
                var fullThumbnailPath = GetFullPath(thumbnailPath);
                if (File.Exists(fullThumbnailPath))
                {
                    File.Delete(fullThumbnailPath);
                }

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در حذف فایل {FilePath}", filePath);
            return false;
        }
    }

    public async Task<bool> DeleteFilesAsync(List<string> filePaths, CancellationToken cancellationToken = default)
    {
        var allSuccess = true;

        foreach (var filePath in filePaths)
        {
            var result = await DeleteFileAsync(filePath, cancellationToken);
            if (!result)
                allSuccess = false;
        }

        return allSuccess;
    }

    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(_baseUrl))
        {
            return $"/{filePath}";
        }

        return $"{_baseUrl.TrimEnd('/')}/{filePath}";
    }

    public async Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(filePath);
        return File.Exists(fullPath);
    }

    public async Task<FileInfo?> GetFileInfoAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var fullPath = GetFullPath(filePath);
        
        if (!File.Exists(fullPath))
            return null;

        var fileInfo = new FileInfo(fullPath);
        return fileInfo;
    }

    public string GenerateUniqueFileName(string originalFileName, string? folder = null)
    {
        var extension = Path.GetExtension(originalFileName);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var uniqueId = Guid.NewGuid().ToString("N")[..8];
        
        return $"{fileNameWithoutExtension}_{timestamp}_{uniqueId}{extension}";
    }

    public async Task<FileValidationResult> ValidateFileAsync(FileUploadRequest fileUploadRequest)
    {
        var result = new FileValidationResult { IsValid = true };

        // بررسی وجود فایل
        if (fileUploadRequest.File == null || fileUploadRequest.File.Length == 0)
        {
            result.AddError("فایل انتخاب نشده است");
            return result;
        }

        // بررسی سایز فایل
        var maxSize = fileUploadRequest.MaxFileSize ?? _defaultMaxFileSize;
        if (fileUploadRequest.File.Length > maxSize)
        {
            result.AddError($"سایز فایل نمی‌تواند بیشتر از {FormatFileSize(maxSize)} باشد");
        }

        // بررسی نوع فایل
        var extension = Path.GetExtension(fileUploadRequest.File.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension))
        {
            result.AddError("فایل باید دارای پسوند معتبر باشد");
            return result;
        }

        // بررسی پسوندهای مجاز
        var allowedExtensions = GetAllowedExtensions(fileUploadRequest);
        if (!allowedExtensions.Contains(extension))
        {
            result.AddError($"نوع فایل مجاز نیست. انواع مجاز: {string.Join(", ", allowedExtensions)}");
        }

        // بررسی محتوای فایل (امنیتی)
        if (!await IsFileContentValidAsync(fileUploadRequest.File))
        {
            result.AddError("محتوای فایل مشکوک است");
        }

        return result;
    }

    public async Task<string?> ResizeImageAsync(string imagePath, int width, int height, int quality = 85, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = GetFullPath(imagePath);
            if (!File.Exists(fullPath))
                return null;

            var resizedPath = GetResizedImagePath(imagePath, width, height);
            var fullResizedPath = GetFullPath(resizedPath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullResizedPath)!);

            using var image = await Image.LoadAsync(fullPath, cancellationToken);
            image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));

            var encoder = new JpegEncoder { Quality = quality };
            await image.SaveAsJpegAsync(fullResizedPath, encoder, cancellationToken);

            return resizedPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در تغییر اندازه تصویر {ImagePath}", imagePath);
            return null;
        }
    }

    public async Task<string?> CreateThumbnailAsync(string imagePath, int thumbnailSize = 150, CancellationToken cancellationToken = default)
    {
        return await ResizeImageAsync(imagePath, thumbnailSize, thumbnailSize, 80, cancellationToken);
    }

    #region Private Methods

    private string GetUploadPath(string? folder)
    {
        var webRoot = GetWebRootPath();
        var basePath = Path.Combine(webRoot, _uploadsPath);
        
        if (!string.IsNullOrEmpty(folder))
        {
            basePath = Path.Combine(basePath, folder);
        }

        return basePath;
    }

    private string GetFullPath(string relativePath)
    {
        var webRoot = GetWebRootPath();
        return Path.Combine(webRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
    }

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

    private string[] GetAllowedExtensions(FileUploadRequest request)
    {
        if (request.AllowedExtensions != null && request.AllowedExtensions.Length > 0)
        {
            return request.AllowedExtensions;
        }

        return request.ExpectedFileType switch
        {
            FileType.Image => _defaultAllowedImageExtensions,
            FileType.Document => _defaultAllowedDocumentExtensions,
            _ => _defaultAllowedImageExtensions.Concat(_defaultAllowedDocumentExtensions).ToArray()
        };
    }

    private bool IsImageFile(string? contentType)
    {
        return contentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true;
    }

    private async Task<bool> IsFileContentValidAsync(IFormFile file)
    {
        // بررسی ابتدایی محتوای فایل برای امنیت
        // می‌توانید بررسی‌های پیشرفته‌تری اضافه کنید
        
        using var stream = file.OpenReadStream();
        var buffer = new byte[1024];
        await stream.ReadAsync(buffer, 0, buffer.Length);

        // بررسی فایل‌های مشکوک
        var content = System.Text.Encoding.UTF8.GetString(buffer);
        var suspiciousPatterns = new[] { "<script", "javascript:", "vbscript:", "onload=", "onerror=" };
        
        return !suspiciousPatterns.Any(pattern => 
            content.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }

    private async Task ProcessImageAsync(FileUploadRequest request, string filePath, FileUploadResult result, CancellationToken cancellationToken)
    {
        try
        {
            // تغییر اندازه تصویر
            if (request.ShouldResize && request.ResizeWidth.HasValue && request.ResizeHeight.HasValue)
            {
                var resizedPath = await ResizeImageAsync(result.FilePath!, request.ResizeWidth.Value, request.ResizeHeight.Value, request.ImageQuality, cancellationToken);
                if (!string.IsNullOrEmpty(resizedPath))
                {
                    // جایگزینی فایل اصلی با نسخه تغییر اندازه یافته
                    var originalPath = GetFullPath(result.FilePath!);
                    var newResizedPath = GetFullPath(resizedPath);
                    
                    File.Delete(originalPath);
                    File.Move(newResizedPath, originalPath);
                }
            }

            // ایجاد thumbnail
            if (request.CreateThumbnail)
            {
                var thumbnailPath = await CreateThumbnailAsync(result.FilePath!, request.ThumbnailSize, cancellationToken);
                if (!string.IsNullOrEmpty(thumbnailPath))
                {
                    result.ThumbnailPath = thumbnailPath;
                    result.ThumbnailUrl = GetFileUrl(thumbnailPath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطا در پردازش تصویر {FilePath}", result.FilePath);
        }
    }

    private string GetThumbnailPath(string originalPath)
    {
        var directory = Path.GetDirectoryName(originalPath);
        var fileName = Path.GetFileNameWithoutExtension(originalPath);
        var extension = Path.GetExtension(originalPath);
        
        return Path.Combine(directory ?? "", "thumbnails", $"{fileName}_thumb{extension}").Replace("\\", "/");
    }

    private string GetResizedImagePath(string originalPath, int width, int height)
    {
        var directory = Path.GetDirectoryName(originalPath);
        var fileName = Path.GetFileNameWithoutExtension(originalPath);
        var extension = Path.GetExtension(originalPath);
        
        return Path.Combine(directory ?? "", "resized", $"{fileName}_{width}x{height}{extension}").Replace("\\", "/");
    }

    private static string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }

    #endregion
} 