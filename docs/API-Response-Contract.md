# قرارداد پاسخ API (API Response Contract)

این سند ساختار استاندارد پاسخ‌های API را در VisitorApp تعریف می‌کند.

## ساختار کلی

```json
{
  "success": true,
  "data": {},
  "message": "Success",
  "code": "SUCCESS",
  "traceId": "<correlation-id>",
  "errors": [],
  "timestamp": "2025-01-01T00:00:00.000Z"
}
```

- success: موفق/ناموفق بودن عملیات
- data: داده اصلی پاسخ (در خطا null است)
- message: پیام انسانی قابل‌خواندن
- code: کد استاندارد شده دامنه/خطا
- traceId: شناسه رهگیری برای لاگ‌ها
- errors: آرایه جزئیات خطا در اعتبارسنجی
- timestamp: زمان تولید پاسخ

## مثال‌ها

### موفق
```json
{
  "success": true,
  "data": { "id": "123", "name": "Product" },
  "message": "Success",
  "code": "SUCCESS",
  "traceId": "ABC123",
  "errors": null,
  "timestamp": "2025-01-01T10:30:45.123Z"
}
```

### خطای عمومی
```json
{
  "success": false,
  "data": null,
  "message": "Product not found",
  "code": "NOT_FOUND",
  "traceId": "ABC124",
  "errors": null,
  "timestamp": "2025-01-01T10:30:45.123Z"
}
```

### خطای اعتبارسنجی
```json
{
  "success": false,
  "data": null,
  "message": "Validation failed",
  "code": "VALIDATION_ERROR",
  "traceId": "ABC125",
  "errors": [
    { "property": "name", "message": "Name is required", "code": "REQUIRED", "attemptedValue": null }
  ],
  "timestamp": "2025-01-01T10:30:45.123Z"
}
```

## پیکربندی
نمونه پیکربندی در `appsettings.json`:
```json
{
  "EnvelopeResponse": {
    "IsEnabled": true,
    "EnableBackwardCompatibility": false,
    "ExcludedEndpoints": ["/api/v1/files"],
    "EnableDetailedErrors": false,
    "DefaultSuccessMessage": "Success",
    "DefaultSuccessCode": "SUCCESS"
  }
}
```

## نکات پیاده‌سازی
- Endpoints جدید از کلاس‌های پایه در `VisitorApp.API/Common/Endpoints` ارث‌بری کنند.
- برای پاسخ خام (دانلود فایل و ...) از مسیر Raw/استثناهای مستند استفاده شود.
- خطاهای دامنه به کدهای استاندارد نگاشت شوند.
