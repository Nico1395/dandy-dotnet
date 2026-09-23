using System.Linq.Expressions;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder Property<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TProperty?>> propertyExpression, string columnName, bool nullable = false)
        where TEntity : class
    {
        return builder.Property(propertyExpression).HasColumnName(columnName).IsRequired(nullable);
    }

    public static PropertyBuilder Property<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, string?>> propertyExpression, string columnName, int maxLength, bool nullable = false)
        where TEntity : class
    {
        return builder.Property(propertyExpression).HasColumnName(columnName).IsRequired(nullable).HasMaxLength(maxLength);
    }

    public static EntityTypeBuilder<TEntity> CreatedAtProperty<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICreatedAt
    {
        builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
        return builder;
    }

    public static EntityTypeBuilder<TEntity> UpdatedAtProperty<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IUpdatedAt
    {
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();
        return builder;
    }

    public static EntityTypeBuilder<TEntity> SoftDeletedAtProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ISoftDeletedAt
    {
        builder.Property(p => p.Deleted).HasColumnName("deleted").IsRequired();
        builder.Property(p => p.DeletedAt).HasColumnName("deleted_at").IsRequired(false);

        return builder;
    }
}