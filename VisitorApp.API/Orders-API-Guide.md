# راهنمای API های Customer و Order

این مستند شامل راهنمای استفاده از API های مدیریت مشتریان و سفارشات است.

## 📋 فهرست مطالب

- [Customer APIs](#customer-apis)
- [Order APIs](#order-apis)
- [نکات مهم](#نکات-مهم)
- [نمونه‌های استفاده](#نمونه‌های-استفاده)

---

## 👥 Customer APIs

### 1. ایجاد مشتری جدید
**POST** `/api/Customers`

**Request Body:**
```json
{
  "fullName": "علی احمدی",
  "phoneNumber": "09123456789",
  "companyName": "شرکت نمونه",
  "isTemporary": false
}
```

**Response:**
```json
{
  "data": {
    "id": "guid",
    "fullName": "علی احمدی",
    "phoneNumber": "09123456789",
    "companyName": "شرکت نمونه",
    "isTemporary": false
  }
}
```

### 2. دریافت مشتری با ID
**GET** `/api/Customers/{id}`

### 3. ویرایش مشتری
**PUT** `/api/Customers/{id}`

**Request Body:**
```json
{
  "id": "guid",
  "fullName": "علی احمدی",
  "phoneNumber": "09123456789",
  "companyName": "شرکت جدید",
  "isTemporary": false
}
```

### 4. دریافت لیست صفحه‌بندی شده مشتریان
**GET** `/api/Customers?page=1&pageSize=10`

**با فیلتر:**
```json
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "fullName": "علی",
    "phoneNumber": "0912",
    "companyName": "شرکت",
    "isTemporary": false
  }
}
```

### 5. حذف مشتری
**DELETE** `/api/Customers/{id}`

---

## 🛒 Order APIs

### 1. ایجاد سفارش جدید (با لیست آیتم‌ها)
**POST** `/api/Orders`

**Request Body:**
```json
{
  "customerId": "guid",
  "orderDate": "2025-09-30T10:00:00Z",
  "status": 0,
  "items": [
    {
      "productId": "guid",
      "quantity": 2,
      "unitPrice": 1500000
    },
    {
      "productId": "guid",
      "quantity": 1,
      "unitPrice": 2500000
    }
  ]
}
```

**Response:**
```json
{
  "data": {
    "id": "guid",
    "orderNumber": "#00001",
    "customerId": "guid",
    "customerName": "علی احمدی",
    "orderDate": "2025-09-30T10:00:00Z",
    "totalAmount": 5500000,
    "status": 0,
    "items": [
      {
        "id": "guid",
        "productId": "guid",
        "productTitle": "لپ تاپ",
        "quantity": 2,
        "unitPrice": 1500000,
        "totalPrice": 3000000
      }
    ]
  }
}
```

### 2. دریافت جزئیات سفارش (شامل لیست آیتم‌ها)
**GET** `/api/Orders/{id}`

**Response:**
```json
{
  "data": {
    "id": "guid",
    "orderNumber": "#00001",
    "customerId": "guid",
    "customerName": "علی احمدی",
    "customerPhoneNumber": "09123456789",
    "orderDate": "2025-09-30T10:00:00Z",
    "totalAmount": 5500000,
    "status": 0,
    "statusText": "Pending",
    "items": [...]
  }
}
```

### 3. ویرایش سفارش (با لیست آیتم‌ها)
**PUT** `/api/Orders/{id}`

**Request Body:**
```json
{
  "id": "guid",
  "customerId": "guid",
  "orderDate": "2025-09-30T10:00:00Z",
  "status": 1,
  "items": [
    {
      "productId": "guid",
      "quantity": 3,
      "unitPrice": 1500000
    }
  ]
}
```

### 4. دریافت لیست صفحه‌بندی شده سفارشات
**GET** `/api/Orders?page=1&pageSize=10`

**با فیلتر:**
```json
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "orderNumber": "#00001",
    "customerId": "guid",
    "status": 0,
    "orderDateFrom": "2025-09-01T00:00:00Z",
    "orderDateTo": "2025-09-30T23:59:59Z"
  }
}
```

### 5. دریافت سفارشات کاربر جاری
**GET** `/api/Orders/my-orders?page=1&pageSize=10`

این API فقط سفارشاتی که توسط کاربر لاگین شده ثبت شده‌اند را برمی‌گرداند.

**با فیلتر:**
```json
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "orderNumber": "#00001",
    "status": 0,
    "orderDateFrom": "2025-09-01T00:00:00Z",
    "orderDateTo": "2025-09-30T23:59:59Z"
  }
}
```

**Response:**
```json
{
  "data": {
    "items": [...],
    "totalCount": 10,
    "page": 1,
    "pageSize": 10
  }
}
```

**نکته مهم:** این API نیاز به احراز هویت دارد اما نیازی به نقش Admin ندارد. هر کاربر لاگین شده می‌تواند سفارشات خودش را مشاهده کند.

### 6. حذف سفارش
**DELETE** `/api/Orders/{id}`

---

## ⚠️ نکات مهم

### وضعیت‌های سفارش (OrderStatus)
- `0` - Pending (در انتظار)
- `1` - Confirmed (تایید شده)
- `2` - Canceled (لغو شده)

### ثبت کاربر ایجاد کننده سفارش
- هنگام ایجاد سفارش، **UserId** کاربر جاری به صورت خودکار ثبت می‌شود
- این فیلد مشخص می‌کند کدام ویزیتور سفارش را ثبت کرده است
- در تمام response ها، اطلاعات کاربر ثبت کننده (`UserFullName`) نمایش داده می‌شود

### مشتری متفرقه
برای ثبت سفارشات بدون مشتری مشخص، می‌توانید یک مشتری متفرقه ایجاد کنید:
```json
{
  "fullName": "مشتری متفرقه",
  "phoneNumber": "00000000000",
  "isTemporary": true
}
```

### مدیریت آیتم‌های سفارش
- در **ایجاد سفارش**: لیست آیتم‌ها الزامی است (حداقل یک آیتم)
- در **ویرایش سفارش**: تمام آیتم‌های قبلی حذف و آیتم‌های جدید جایگزین می‌شوند
- در **دریافت جزئیات**: تمام آیتم‌ها به همراه اطلاعات محصول برگردانده می‌شود
- **قیمت کل** به صورت خودکار محاسبه می‌شود

### اعتبارسنجی
- نام و نام خانوادگی: الزامی، حداکثر 200 کاراکتر
- شماره تلفن: الزامی، حداکثر 20 کاراکتر
- نام شرکت: اختیاری، حداکثر 200 کاراکتر
- تعداد آیتم: باید بزرگتر از صفر باشد
- قیمت واحد: نمی‌تواند منفی باشد

---

## 💡 نمونه‌های استفاده

### سناریو 1: ثبت سفارش کامل

```bash
# 1. ایجاد مشتری
POST /api/Customers
{
  "fullName": "رضا محمدی",
  "phoneNumber": "09121234567",
  "companyName": "شرکت تست",
  "isTemporary": false
}
# Response: customerId = "abc123..."

# 2. ایجاد سفارش برای این مشتری
POST /api/Orders
{
  "customerId": "abc123...",
  "orderDate": "2025-09-30T10:00:00Z",
  "status": 0,
  "items": [
    {
      "productId": "xyz456...",
      "quantity": 2,
      "unitPrice": 1500000
    }
  ]
}
# Response: orderId = "order789...", orderNumber = "#00001"

# 3. مشاهده جزئیات سفارش
GET /api/Orders/order789...
```

### سناریو 2: ویرایش سفارش

```bash
# 1. ویرایش سفارش و تغییر آیتم‌ها
PUT /api/Orders/order789...
{
  "id": "order789...",
  "customerId": "abc123...",
  "orderDate": "2025-09-30T10:00:00Z",
  "status": 1,
  "items": [
    {
      "productId": "xyz456...",
      "quantity": 5,
      "unitPrice": 1500000
    },
    {
      "productId": "def789...",
      "quantity": 1,
      "unitPrice": 3000000
    }
  ]
}
```

### سناریو 3: جستجوی سفارشات

```bash
# جستجوی سفارشات یک مشتری خاص (فقط Admin)
GET /api/Orders
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "customerId": "abc123...",
    "status": 0
  }
}

# جستجوی سفارشات بر اساس بازه زمانی (فقط Admin)
GET /api/Orders
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "orderDateFrom": "2025-09-01T00:00:00Z",
    "orderDateTo": "2025-09-30T23:59:59Z"
  }
}
```

### سناریو 4: مشاهده سفارشات شخصی (کاربر عادی)

```bash
# هر کاربر می‌تواند فقط سفارشات خودش را ببیند
GET /api/Orders/my-orders?page=1&pageSize=10

# با فیلتر وضعیت
GET /api/Orders/my-orders
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "status": 0
  }
}

# با فیلتر بازه زمانی
GET /api/Orders/my-orders
{
  "page": 1,
  "pageSize": 10,
  "filter": {
    "orderDateFrom": "2025-09-01T00:00:00Z",
    "orderDateTo": "2025-09-30T23:59:59Z"
  }
}
```

---

## 🔧 تست API ها

برای تست راحت‌تر API ها می‌توانید از فایل‌های `.http` موجود استفاده کنید:
- `Customers.http` - تست API های مشتری
- `Orders.http` - تست API های سفارش

این فایل‌ها در Visual Studio Code با افزونه REST Client یا در Rider قابل اجرا هستند.

---

## 📝 یادداشت‌ها

### سطوح دسترسی
- **Admin APIs**: ایجاد، ویرایش، حذف و مشاهده همه سفارشات (نیاز به Role: Admin)
- **User API**: مشاهده سفارشات شخصی `/api/Orders/my-orders` (هر کاربر احراز هویت شده)

### نکات مهم
- شماره سفارش به صورت خودکار و یکتا تولید می‌شود
- **UserId** کاربر ایجاد کننده به صورت خودکار ثبت می‌شود
- در صورت حذف سفارش، تمام آیتم‌های آن نیز حذف می‌شوند (Cascade Delete)
- حذف مشتری در صورت وجود سفارش امکان‌پذیر نیست (Restrict Delete)
- حذف کاربر، سفارشات مرتبط را حذف نمی‌کند (UserId به null تبدیل می‌شود)
