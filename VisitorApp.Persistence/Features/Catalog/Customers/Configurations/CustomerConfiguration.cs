using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Persistence.Features.Catalog.Customers.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.CompanyName)
            .HasMaxLength(200);

        builder.Property(c => c.IsTemporary)
            .IsRequired()
            .HasDefaultValue(false);

        // برای مشتریان دائمی، شماره تلفن باید یکتا باشد
        // برای مشتریان متفرقه محدودیتی نیست
        builder.HasIndex(c => c.PhoneNumber)
            .HasFilter("[IsTemporary] = 0")
            .IsUnique();

        // Relationship with Orders
        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}