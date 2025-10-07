using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluralize.NET;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Persistence.Features.Catalog.Categories.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var pluralizer = new Pluralizer();
        string tableName = pluralizer.Pluralize(nameof(Category));
        builder.ToTable(tableName);

        builder.HasQueryFilter(d => d.RemovedAt == null);

        builder.HasQueryFilter(d => d.RemovedAt == null);

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.Version).IsConcurrencyToken();

        // Create unique index on Name for active categories
        builder.HasIndex(d => d.Name)
            .IsUnique()
            .HasFilter($"{nameof(Category.RemovedAt)} IS NULL");
    }
} 