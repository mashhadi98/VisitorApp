using Microsoft.AspNetCore.Http;

namespace VisitorApp.Application.Common.DTOs;

/// <summary>
/// درخواست آپلود فایل
/// </summary>
public class FileUploadRequest
{
    /// <summary>
    /// فایل آپلود شده
    /// </summary>
    public required IFormFile File { get; set; }

    /// <summary>
    /// پوشه مقصد (اختیاری)
    /// </summary>
    public string? Folder { get; set; }

    /// <summary>
    /// نام دلخواه فایل (اختیاری)
    /// </summary>
    public string? CustomFileName { get; set; }

    /// <summary>
    /// آیا نام فایل منحصر به فرد ایجاد شود؟
    /// </summary>
    public bool GenerateUniqueFileName { get; set; } = true;

    /// <summary>
    /// نوع فایل مورد انتظار
    /// </summary>
    public FileType? ExpectedFileType { get; set; }

    /// <summary>
    /// حداکثر سایز فایل (بایت)
    /// </summary>
    public long? MaxFileSize { get; set; }

    /// <summary>
    /// انواع فایل مجاز
    /// </summary>
    public string[]? AllowedExtensions { get; set; }

    /// <summary>
    /// آیا تصویر باید تغییر اندازه پیدا کند؟
    /// </summary>
    public bool ShouldResize { get; set; } = false;

    /// <summary>
    /// عرض تصویر برای تغییر اندازه
    /// </summary>
    public int? ResizeWidth { get; set; }

    /// <summary>
    /// ارتفاع تصویر برای تغییر اندازه
    /// </summary>
    public int? ResizeHeight { get; set; }

    /// <summary>
    /// کیفیت تصویر (0-100)
    /// </summary>
    public int ImageQuality { get; set; } = 85;

    /// <summary>
    /// آیا thumbnail ایجاد شود؟
    /// </summary>
    public bool CreateThumbnail { get; set; } = false;

    /// <summary>
    /// اندازه thumbnail
    /// </summary>
    public int ThumbnailSize { get; set; } = 150;
}

/// <summary>
/// انواع فایل
/// </summary>
public enum FileType
{
    /// <summary>
    /// تصویر
    /// </summary>
    Image = 1,

    /// <summary>
    /// سند
    /// </summary>
    Document = 2,

    /// <summary>
    /// ویدیو
    /// </summary>
    Video = 3,

    /// <summary>
    /// صوت
    /// </summary>
    Audio = 4,

    /// <summary>
    /// فایل فشرده
    /// </summary>
    Archive = 5,

    /// <summary>
    /// سایر
    /// </summary>
    Other = 999
} 