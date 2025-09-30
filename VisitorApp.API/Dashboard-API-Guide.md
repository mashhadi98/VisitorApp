# راهنمای API های داشبورد

این مستند شامل راهنمای استفاده از API های داشبورد و گزارش‌گیری است.

## 📋 فهرست مطالب

- [Dashboard Summary](#dashboard-summary)
- [Quick Order](#quick-order)
- [Sales Report](#sales-report)
- [Order Actions](#order-actions)
- [نمونه‌های استفاده](#نمونه‌های-استفاده)

---

## 📊 Dashboard Summary

### دریافت خلاصه داشبورد
**GET** `/api/dashboard/summary`

این API اطلاعات کلی داشبورد را برمی‌گرداند.

**Query Parameters:**
- `month` (اختیاری): ماه مورد نظر به فرمت `YYYY-MM` (مثال: `2025-09`)
  - اگر ارسال نشود، ماه جاری استفاده می‌شود

**Response:**
```json
{
  "data": {
    "usersCount": 124,
    "productsCount": 89,
    "ordersThisMonth": 42,
    "salesThisMonth": 225000000
  }
}
```

**نمونه استفاده:**
```http
GET /api/dashboard/summary
GET /api/dashboard/summary?month=2025-09
```

---

## ⚡ Quick Order (ثبت سریع سفارش)

### ثبت سریع سفارش
**POST** `/api/Orders/quick`

این API امکان ثبت سریع سفارش را فراهم می‌کند با قابلیت‌های زیر:
- ثبت سفارش با مشتری موجود
- ثبت سفارش با ایجاد مشتری جدید
- ثبت سفارش به نام کاربر دیگر (فقط Admin)

**Request Body:**
```json
{
  "userId": "guid|null",           // اختیاری: فقط Admin می‌تواند
  "customerId": "guid|null",       // استفاده از مشتری موجود
  "newCustomer": {                 // یا ایجاد مشتری جدید
    "fullName": "احمد رضایی",
    "phoneNumber": "09123456789",
    "companyName": "شرکت تست",     // اختیاری
    "isTemporary": false
  },
  "items": [
    {
      "productId": "guid",
      "quantity": 2,
      "unitPrice": 1500000
    }
  ],
  "note": "توضیحات اختیاری"      // اختیاری
}
```

**Response (201 Created):**
```json
{
  "data": {
    "orderId": "guid",
    "orderNumber": "#00001",
    "userId": "guid",
    "status": 0,
    "totalAmount": 3000000
  }
}
```

**نکات مهم:**
- باید حداقل یکی از `customerId` یا `newCustomer` ارسال شود
- `userId` فقط توسط Admin قابل تنظیم است
- اگر `userId` ارسال نشود، UserId کاربر جاری استفاده می‌شود
- حداقل یک آیتم الزامی است

**Errors:**
- `400 Bad Request`: ورودی نامعتبر
- `403 Forbidden`: عدم دسترسی برای ثبت سفارش به نام کاربر دیگر
- `404 Not Found`: مشتری یا محصول یافت نشد

---

## 📈 Sales Report (گزارش فروش)

### گزارش فروش ماهانه
**GET** `/api/reports/sales/monthly`

دریافت گزارش فروش چند ماه گذشته.

**Query Parameters:**
- `months` (اختیاری، پیش‌فرض: 6): تعداد ماه گذشته (حداکثر 12)
- `year` (اختیاری): سال مورد نظر (مثال: 2025)

**Response:**
```json
{
  "data": {
    "series": [
      {
        "label": "فروردین 2025",
        "value": 12500000
      },
      {
        "label": "اردیبهشت 2025",
        "value": 18000000
      }
    ],
    "total": 30500000
  }
}
```

**نمونه استفاده:**
```http
GET /api/reports/sales/monthly
GET /api/reports/sales/monthly?months=6
GET /api/reports/sales/monthly?months=12&year=2025
```

---

## ✅ Order Actions (عملیات روی سفارش)

### 1. تایید سفارش
**POST** `/api/Orders/{id}/confirm`

تایید یک سفارش و تغییر وضعیت آن به Confirmed.

**Path Parameters:**
- `id`: شناسه سفارش

**Response (200 OK):**
```json
{
  "data": {
    "success": true,
    "message": "سفارش با موفقیت تایید شد"
  }
}
```

**Errors:**
- `404 Not Found`: سفارش یافت نشد
- `400 Bad Request`: سفارش قبلاً تایید شده یا لغو شده است

---

### 2. لغو سفارش
**POST** `/api/Orders/{id}/cancel`

لغو یک سفارش و تغییر وضعیت آن به Canceled.

**Path Parameters:**
- `id`: شناسه سفارش

**Response (200 OK):**
```json
{
  "data": {
    "success": true,
    "message": "سفارش با موفقیت لغو شد"
  }
}
```

**Errors:**
- `404 Not Found`: سفارش یافت نشد
- `400 Bad Request`: سفارش قبلاً لغو شده است

---

## 💡 نمونه‌های استفاده

### سناریو 1: داشبورد کامل

```bash
# 1. دریافت خلاصه داشبورد
GET /api/dashboard/summary
Response: {
  "usersCount": 50,
  "productsCount": 120,
  "ordersThisMonth": 35,
  "salesThisMonth": 180000000
}

# 2. مشاهده گزارش فروش 6 ماه اخیر
GET /api/reports/sales/monthly?months=6
Response: {
  "series": [...],
  "total": 450000000
}
```

### سناریو 2: ثبت سریع سفارش با مشتری جدید

```bash
# ثبت سفارش
POST /api/Orders/quick
{
  "newCustomer": {
    "fullName": "علی محمدی",
    "phoneNumber": "09121234567",
    "isTemporary": false
  },
  "items": [
    {
      "productId": "abc-123...",
      "quantity": 2,
      "unitPrice": 1500000
    }
  ]
}

Response: {
  "orderId": "xyz-789...",
  "orderNumber": "#00042",
  "userId": "current-user-id",
  "status": 0,
  "totalAmount": 3000000
}
```

### سناریو 3: مدیریت سفارش

```bash
# 1. ثبت سفارش سریع
POST /api/Orders/quick
{...}
Response: { "orderId": "order-123..." }

# 2. مشاهده جزئیات سفارش
GET /api/Orders/order-123...

# 3. تایید سفارش
POST /api/Orders/order-123.../confirm
Response: { "success": true }

# یا لغو سفارش
POST /api/Orders/order-123.../cancel
Response: { "success": true }
```

### سناریو 4: Admin ثبت سفارش به نام ویزیتور

```bash
# Admin می‌تواند سفارش را به نام کاربر دیگر ثبت کند
POST /api/Orders/quick
{
  "userId": "visitor-user-id",
  "customerId": "customer-123...",
  "items": [
    {
      "productId": "product-456...",
      "quantity": 1,
      "unitPrice": 2000000
    }
  ]
}
```

---

## 🔐 سطوح دسترسی

| API | نقش مورد نیاز | توضیحات |
|-----|--------------|---------|
| Dashboard Summary | Admin | دسترسی کامل به آمار |
| Quick Order | Admin | ثبت سفارش سریع |
| Sales Report | Admin | گزارش فروش |
| Confirm Order | Admin | تایید سفارش |
| Cancel Order | Admin | لغو سفارش |

---

## 📝 نکات مهم

### Quick Order
- اگر `customerId` و `newCustomer` هر دو ارسال شوند، `customerId` اولویت دارد
- `userId` فقط توسط Admin قابل تنظیم است
- اگر `userId` خالی باشد، کاربر جاری استفاده می‌شود
- مشتری جدید به صورت خودکار ذخیره می‌شود

### Order Status
- `0` = Pending (در انتظار)
- `1` = Confirmed (تایید شده)
- `2` = Canceled (لغو شده)

### Sales Report
- گزارش بر اساس `OrderDate` محاسبه می‌شود
- ماه‌های فارسی نمایش داده می‌شوند
- حداکثر 12 ماه قابل دریافت است

---

## 🔧 تست API ها

فایل `Dashboard.http` شامل نمونه‌های کامل تست است که در Visual Studio Code (با افزونه REST Client) یا Rider قابل اجرا است.

---

## 🐛 خطاهای رایج

### 400 Bad Request
- ورودی‌های الزامی ارسال نشده
- فرمت ورودی‌ها نامعتبر است
- سفارش قبلاً تایید یا لغو شده

### 403 Forbidden
- کاربر عادی نمی‌تواند `userId` تنظیم کند
- فقط Admin می‌تواند سفارش را به نام دیگران ثبت کند

### 404 Not Found
- سفارش، مشتری یا محصول یافت نشد
- ID های ارسالی نامعتبر هستند

