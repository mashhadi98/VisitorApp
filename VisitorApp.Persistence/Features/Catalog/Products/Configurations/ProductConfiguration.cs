using VisitorApp.Domain.Features.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Helpers;

namespace VisitorApp.Persistence.Features.Catalog.Products.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product).ToSnakeCase());

        builder.HasQueryFilter(d => d.RemovedAt == null);

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName($"{nameof(Product)}{nameof(Product.Id)}".ToSnakeCase()).HasColumnType("uuid");
        builder.Property(d => d.Title).HasColumnName(nameof(Product.Title).ToSnakeCase()).HasColumnType("varchar(255)");
        builder.Property(d => d.Description).HasColumnName(nameof(Product.Description).ToSnakeCase()).HasColumnType("varchar(255)");
        builder.Property(d => d.IsActive).HasColumnName(nameof(Product.IsActive).ToSnakeCase());

        builder.Property(d => d.CreatedAt).HasColumnName(nameof(Product.CreatedAt).ToSnakeCase()).HasColumnType("datetime");
        builder.Property(d => d.UpdatedAt).HasColumnName(nameof(Product.UpdatedAt).ToSnakeCase()).HasColumnType("datetime");
        builder.Property(d => d.RemovedAt).HasColumnName(nameof(Product.RemovedAt).ToSnakeCase()).HasColumnType("datetime");
        
        // Configure Version as Concurrency Token
        builder.Property(d => d.Version).IsConcurrencyToken();
    }
}