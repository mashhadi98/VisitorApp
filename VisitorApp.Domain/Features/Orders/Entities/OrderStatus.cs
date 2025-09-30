namespace VisitorApp.Domain.Features.Orders.Entities;

public enum OrderStatus
{
    Pending = 0,   // در انتظار
    Confirmed = 1, // تایید شده
    Canceled = 2   // لغو شده (اختیاری)
}