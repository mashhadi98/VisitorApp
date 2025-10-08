# Order Creation Bug Fix

## مشکل (Problem)
هنگام ثبت سفارش جدید، خطای 500 در backend رخ می‌داد و Entity Framework نمی‌توانست تغییرات را در دیتابیس ذخیره کند.

When creating a new order, a 500 error occurred in the backend and Entity Framework could not save changes to the database.

## علت (Root Cause)
در فایل `CreateOrderCommandHandler.cs` در خط 56، هنگام ساخت `OrderItem`، مقدار `OrderId` به صورت صریح تنظیم می‌شد:

```csharp
var orderItem = new OrderItem
{
    OrderId = order.Id,  // ❌ مشکل اینجا بود
    ProductId = itemDto.ProductId,
    Quantity = itemDto.Quantity,
    UnitPrice = itemDto.UnitPrice
};
```

مشکل این بود که:
1. `order.Id` هنوز مقدار پیش‌فرض (Guid خالی) داشت چون سفارش هنوز ذخیره نشده بود
2. وقتی آیتم را به کالکشن `order.Items` اضافه می‌کردیم، Entity Framework تلاش می‌کرد رابطه را به صورت خودکار مدیریت کند
3. این تضاد باعث خطا در SaveChanges می‌شد

The problem was that:
1. `order.Id` still had the default value (empty Guid) because the order hadn't been saved yet
2. When we added the item to the `order.Items` collection, Entity Framework tried to manage the relationship automatically
3. This conflict caused an error during SaveChanges

## راه‌حل (Solution)
خط تنظیم `OrderId` حذف شد و به Entity Framework اجازه داده شد که رابطه را به صورت خودکار مدیریت کند:

```csharp
var orderItem = new OrderItem
{
    // ✅ OrderId را حذف کردیم - EF Core خودش مدیریت می‌کند
    ProductId = itemDto.ProductId,
    Quantity = itemDto.Quantity,
    UnitPrice = itemDto.UnitPrice
};

order.Items.Add(orderItem);  // EF Core به صورت خودکار OrderId را تنظیم می‌کند
```

## فایل تغییر یافته (Changed File)
- `VisitorApp.Api\VisitorApp.Application\Features\Orders\Create\CreateOrderCommandHandler.cs`

## تست (Testing)
برای تست این تغییر، از همان دیتایی که کاربر استفاده کرده بود استفاده کنید:

```json
{
    "customerId": "893f311a-7e71-4fbd-b022-13268c5fb5e4",
    "items": [
        {
            "productId": "746ccd2a-37c0-42aa-9ec6-bc51faf8cf56",
            "quantity": 1,
            "unitPrice": 100000.00
        }
    ]
}
```

اکنون باید سفارش با موفقیت ایجاد شود و خطای 500 دیگر رخ ندهد.

Now the order should be created successfully and the 500 error should no longer occur.

## نکته مهم (Important Note)
این یک الگوی رایج در Entity Framework است: وقتی از Navigation Properties (مثل `order.Items`) استفاده می‌کنید، نیازی به تنظیم صریح Foreign Key نیست. EF Core این کار را به صورت خودکار انجام می‌دهد.

This is a common pattern in Entity Framework: when using Navigation Properties (like `order.Items`), you don't need to explicitly set the Foreign Key. EF Core handles this automatically.

