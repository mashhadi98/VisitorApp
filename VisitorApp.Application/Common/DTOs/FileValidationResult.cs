namespace VisitorApp.Application.Common.DTOs;

/// <summary>
/// نتیجه اعتبارسنجی فایل
/// </summary>
public class FileValidationResult
{
    /// <summary>
    /// آیا فایل معتبر است؟
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// پیام‌های خطا
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// هشدارها
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// اطلاعات اضافی
    /// </summary>
    public Dictionary<string, object> AdditionalInfo { get; set; } = new();

    /// <summary>
    /// ایجاد نتیجه معتبر
    /// </summary>
    public static FileValidationResult Valid(List<string>? warnings = null)
    {
        return new FileValidationResult
        {
            IsValid = true,
            Warnings = warnings ?? new List<string>()
        };
    }

    /// <summary>
    /// ایجاد نتیجه نامعتبر
    /// </summary>
    public static FileValidationResult Invalid(List<string> errors, List<string>? warnings = null)
    {
        return new FileValidationResult
        {
            IsValid = false,
            Errors = errors,
            Warnings = warnings ?? new List<string>()
        };
    }

    /// <summary>
    /// ایجاد نتیجه نامعتبر با یک خطا
    /// </summary>
    public static FileValidationResult Invalid(string error)
    {
        return new FileValidationResult
        {
            IsValid = false,
            Errors = new List<string> { error }
        };
    }

    /// <summary>
    /// اضافه کردن خطا
    /// </summary>
    public void AddError(string error)
    {
        Errors.Add(error);
        IsValid = false;
    }

    /// <summary>
    /// اضافه کردن هشدار
    /// </summary>
    public void AddWarning(string warning)
    {
        Warnings.Add(warning);
    }
} 