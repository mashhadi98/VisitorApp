using Pluralize.NET;
using System.Reflection;

namespace VisitorApp.API.Common.AutoMapperConfiguration;

public class AutoResponseProfile : Profile
{
    public AutoResponseProfile()
    {
        var appAssembly = Assembly.GetAssembly(typeof(VisitorApp.Application.AssemblyReference));
        var domainAssembly = Assembly.GetAssembly(typeof(VisitorApp.Domain.AssemblyReference));

        var responses = appAssembly!.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Response"));

        foreach (var resp in responses)
        {
            Type? entityType = null;

            if (resp.Name.EndsWith("CommandResponse"))
            {
                // CreateProductCommandResponse -> CreateProduct
                var coreName = resp.Name.Replace("CommandResponse", "");

                // آخرین بخش namespace همان Action است (Create/Update/Delete/...)
                // VisitorApp.Application.Features.Catalog.Products.Create
                var actionPart = resp.Namespace?.Split('.').Last() ?? string.Empty;


                // حذف اکشن از ابتدای نام برای رسیدن به Entity
                // CreateProduct -> Product
                var entityName = coreName.StartsWith(actionPart)
                    ? coreName.Substring(actionPart.Length)
                    : coreName;

                var pluralizer = new Pluralizer();
                string singular = pluralizer.Singularize(entityName); // Category

                entityType = domainAssembly!.GetTypes()
                    .FirstOrDefault(t => t.IsClass && t.Name == singular);
            }
            else if (resp.Name.EndsWith("QueryResponse"))
            {
                // CreateProductCommandResponse -> CreateProduct
                var coreName = resp.Name.Replace("QueryResponse", "");

                // آخرین بخش namespace همان Action است (Create/Update/Delete/...)
                // VisitorApp.Application.Features.Catalog.Products.Create
                var actionPart = resp.Namespace?.Split('.').Last() ?? string.Empty;


                // حذف اکشن از ابتدای نام برای رسیدن به Entity
                // CreateProduct -> Product
                var entityName = coreName.StartsWith(actionPart)
                    ? coreName.Substring(actionPart.Length)
                    : coreName;

                var pluralizer = new Pluralizer();
                string singular = pluralizer.Singularize(entityName); // Category

                entityType = domainAssembly!.GetTypes()
                    .FirstOrDefault(t => t.IsClass && t.Name == singular);
            }
         
            if (entityType != null)
            {
                //CreateMap(resp, entityType);
                CreateMap(entityType, resp);
            }
        }
    }
}