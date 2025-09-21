﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pluralize.NET;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Persistence.Features.Catalog.Products.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        var pluralizer = new Pluralizer();
        string tableName = pluralizer.Pluralize(nameof(Product));
        builder.ToTable(tableName);

        builder.HasQueryFilter(d => d.RemovedAt == null);

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Version).IsConcurrencyToken();
    }
}