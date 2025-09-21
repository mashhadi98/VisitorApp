namespace VisitorApp.Application.Common.DTOs;

/// <summary>
/// نتیجه آپلود فایل
/// </summary>
public class FileUploadResult
{
    /// <summary>
    /// آیا آپلود موفق بوده؟
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// مسیر فایل آپلود شده
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// نام فایل
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// URL فایل
    /// </summary>
    public string? FileUrl { get; set; }

    /// <summary>
    /// سایز فایل (بایت)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// نوع فایل (MIME Type)
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// پسوند فایل
    /// </summary>
    public string? FileExtension { get; set; }

    /// <summary>
    /// مسیر thumbnail (در صورت وجود)
    /// </summary>
    public string? ThumbnailPath { get; set; }

    /// <summary>
    /// URL thumbnail (در صورت وجود)
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// پیام خطا (در صورت شکست)
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// جزئیات خطا
    /// </summary>
    public List<string>? ErrorDetails { get; set; }

    /// <summary>
    /// متادیتای اضافی فایل
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// تاریخ آپلود
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// ایجاد نتیجه موفق
    /// </summary>
    public static FileUploadResult Success(string filePath, string fileName, string fileUrl, long fileSize, string? contentType, string? thumbnailPath = null, string? thumbnailUrl = null)
    {
        return new FileUploadResult
        {
            IsSuccess = true,
            FilePath = filePath,
            FileName = fileName,
            FileUrl = fileUrl,
            FileSize = fileSize,
            ContentType = contentType,
            FileExtension = Path.GetExtension(fileName),
            ThumbnailPath = thumbnailPath,
            ThumbnailUrl = thumbnailUrl,
            UploadedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// ایجاد نتیجه ناموفق
    /// </summary>
    public static FileUploadResult Failure(string errorMessage, List<string>? errorDetails = null)
    {
        return new FileUploadResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
            ErrorDetails = errorDetails
        };
    }
} 