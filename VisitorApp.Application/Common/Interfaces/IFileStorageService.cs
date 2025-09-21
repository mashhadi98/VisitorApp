using VisitorApp.Application.Common.DTOs;

namespace VisitorApp.Application.Common.Interfaces;

/// <summary>
/// سرویس مدیریت فایل‌ها و تصاویر
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// آپلود فایل
    /// </summary>
    /// <param name="fileUploadRequest">درخواست آپلود فایل</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>نتیجه آپلود فایل</returns>
    Task<FileUploadResult> UploadFileAsync(FileUploadRequest fileUploadRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// آپلود چندین فایل
    /// </summary>
    /// <param name="fileUploadRequests">لیست درخواست‌های آپلود</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>لیست نتایج آپلود</returns>
    Task<List<FileUploadResult>> UploadFilesAsync(List<FileUploadRequest> fileUploadRequests, CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف فایل
    /// </summary>
    /// <param name="filePath">مسیر فایل</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>نتیجه حذف</returns>
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف چندین فایل
    /// </summary>
    /// <param name="filePaths">لیست مسیر فایل‌ها</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>نتیجه حذف</returns>
    Task<bool> DeleteFilesAsync(List<string> filePaths, CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت URL فایل
    /// </summary>
    /// <param name="filePath">مسیر فایل</param>
    /// <returns>URL فایل</returns>
    string GetFileUrl(string filePath);

    /// <summary>
    /// بررسی وجود فایل
    /// </summary>
    /// <param name="filePath">مسیر فایل</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>وجود دارد یا نه</returns>
    Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات فایل
    /// </summary>
    /// <param name="filePath">مسیر فایل</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>اطلاعات فایل</returns>
    Task<FileInfo?> GetFileInfoAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// تولید نام فایل منحصر به فرد
    /// </summary>
    /// <param name="originalFileName">نام اصلی فایل</param>
    /// <param name="folder">پوشه مقصد</param>
    /// <returns>نام فایل منحصر به فرد</returns>
    string GenerateUniqueFileName(string originalFileName, string? folder = null);

    /// <summary>
    /// اعتبارسنجی فایل
    /// </summary>
    /// <param name="fileUploadRequest">درخواست آپلود</param>
    /// <returns>نتیجه اعتبارسنجی</returns>
    Task<FileValidationResult> ValidateFileAsync(FileUploadRequest fileUploadRequest);

    /// <summary>
    /// تغییر اندازه تصویر
    /// </summary>
    /// <param name="imagePath">مسیر تصویر</param>
    /// <param name="width">عرض</param>
    /// <param name="height">ارتفاع</param>
    /// <param name="quality">کیفیت (0-100)</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>مسیر تصویر تغییر اندازه یافته</returns>
    Task<string?> ResizeImageAsync(string imagePath, int width, int height, int quality = 85, CancellationToken cancellationToken = default);

    /// <summary>
    /// ایجاد thumbnail برای تصویر
    /// </summary>
    /// <param name="imagePath">مسیر تصویر</param>
    /// <param name="thumbnailSize">اندازه thumbnail</param>
    /// <param name="cancellationToken">توکن لغو</param>
    /// <returns>مسیر thumbnail</returns>
    Task<string?> CreateThumbnailAsync(string imagePath, int thumbnailSize = 150, CancellationToken cancellationToken = default);
} 