using VisitorApp.Domain.Features.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Helpers;

namespace VisitorApp.Persistence.Features.Audit.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable(nameof(AuditLog).ToSnakeCase());

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName($"{nameof(AuditLog)}{nameof(AuditLog.Id)}".ToSnakeCase()).HasColumnType("uuid");
        
        builder.Property(d => d.UserId).HasColumnName(nameof(AuditLog.UserId).ToSnakeCase()).HasColumnType("varchar(256)");
        builder.Property(d => d.Type).HasColumnName(nameof(AuditLog.Type).ToSnakeCase()).HasColumnType("varchar(50)");
        builder.Property(d => d.TableName).HasColumnName(nameof(AuditLog.TableName).ToSnakeCase()).HasColumnType("varchar(50)");
        builder.Property(d => d.DateTime).HasColumnName(nameof(AuditLog.DateTime).ToSnakeCase()).HasColumnType("datetime");
        builder.Property(d => d.OldValues).HasColumnName(nameof(AuditLog.OldValues).ToSnakeCase()).HasColumnType("json");
        builder.Property(d => d.NewValues).HasColumnName(nameof(AuditLog.NewValues).ToSnakeCase()).HasColumnType("json");
        builder.Property(d => d.AffectedColumns).HasColumnName(nameof(AuditLog.AffectedColumns).ToSnakeCase()).HasColumnType("json");
        builder.Property(d => d.PrimaryKey).HasColumnName(nameof(AuditLog.PrimaryKey).ToSnakeCase()).HasColumnType("json");

    }
}