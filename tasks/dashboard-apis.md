# Dashboard APIs

این فایل شامل آدرس APIها، ورودی و خروجی‌ها و توضیح عملکرد آن‌ها است.

---

## 1) Summary (کارت‌های شاخص)
**GET** `/api/dashboard/summary`  
دریافت اطلاعات کلی داشبورد (تعداد کاربران، تعداد محصولات، تعداد سفارش‌ها و مجموع فروش).

### Query Parameters
- `month` (اختیاری) : مثلا `2025-09`، پیش‌فرض ماه جاری

### Response
```json
{
  "usersCount": 124,
  "productsCount": 89,
  "ordersThisMonth": 42,
  "salesThisMonth": 225000000
}
```

---

## 2) Orders List (سفارشات در انتظار تأیید)
**GET** `/api/orders`  
لیست سفارش‌ها با قابلیت فیلتر و جستجو.

### Query Parameters
- `status` : pending / confirmed / canceled
- `search` : متن جستجو (اختیاری)
- `page` , `pageSize`
- `sort` : مثلا `createdAt_desc`

### Response
```json
{
  "list": [
    {
      "id": "guid",
      "orderNumber": "#12345",
      "customerName": "احمد محمدی",
      "phone": "0912...",
      "totalAmount": 28500000,
      "status": "Pending",
      "createdAt": "2025-09-15T10:15:00Z"
    }
  ],
  "count": 37
}
```

## 4) Quick Order (ثبت سریع سفارش)
**POST** `/api/orders/quick`  
ثبت سریع سفارش. اگر `userId` ارسال شود سفارش به نام آن کاربر ثبت می‌شود (نیازمند مجوز مناسب).

### Request Body
```json
{
  "userId": "guid | null",
  "customerId": "guid | null",
  "newCustomer": {
    "fullName": "string",
    "phoneNumber": "string",
    "companyName": "string|null",
    "isTemporary": true
  },
  "items": [
    { "productId": "guid", "quantity": 2, "unitPrice": 8500000 }
  ],
  "note": "string|null"
}
```

### Response (201)
```json
{
  "orderId": "guid",
  "orderNumber": "#12346",
  "userId": "guid",
  "status": "Pending",
  "totalAmount": 17000000
}
```

### Errors
- `400`: ورودی نامعتبر
- `403`: عدم دسترسی برای ثبت سفارش به نام کاربر دیگر
- `404`: مشتری یا محصول یافت نشد

### Dropdown APIs
- **GET** `/api/customers?search=...&page=1&pageSize=10`
- **GET** `/api/products?search=...&page=1&pageSize=10`

---

## 5) Sales Report (نمودار فروش ماهانه)
**GET** `/api/reports/sales/monthly`  
دریافت گزارش فروش در بازه زمانی.

### Query Parameters
- `months` : تعداد ماه گذشته (مثلا 6)
- `year` : مثلا 2025

### Response
```json
{
  "series": [
    { "label": "فروردین", "value": 12500000 },
    { "label": "اردیبهشت", "value": 18000000 }
  ],
  "total": 30500000
}
```

---

## 6) Order Actions (عملیات روی سفارش)
- **POST** `/api/orders/{id}/confirm` → `204`
- **POST** `/api/orders/{id}/cancel`  → `204`
- **GET**  `/api/orders/{id}` → دریافت جزئیات سفارش
