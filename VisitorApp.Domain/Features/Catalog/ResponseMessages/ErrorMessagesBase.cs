namespace VisitorApp.Domain.Common.ResponseMessages;
public partial class ErrorMessages
{
    public static class Products
    {
        public static string NotFound = "محصول مورد نظر یافت نشد";
        public static string TitleNotFound = "عنوان محصول اجباری است";
        public static string DescriptionNotFound = "توضیحات محصول اجباری است";
    }

    public static class Categories
    {
        public static string NotFound = "دسته بندی مورد نظر یافت نشد";
        public static string NameNotFound = "نام دسته بندی اجباری است";
        public static string NameAlreadyExists = "نام دسته بندی قبلاً وجود دارد";
        public static string HasRelatedProducts = "این دسته بندی دارای محصول مرتبط می باشد و قابل حذف نیست";
    }
}
