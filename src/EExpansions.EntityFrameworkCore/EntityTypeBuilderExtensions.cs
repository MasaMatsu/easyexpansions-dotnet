using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace EExpansions.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Add a LINQ predicate expression that will automatically be applied to any queries targeting this entity type.
    /// <para>It can be called multiple times and expressions are combined with AND.</para>
    /// <para>This method is an extended version of <see cref="EntityTypeBuilder.HasQueryFilter(LambdaExpression?)"/>.</para>
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
    /// <param name="entityTypeBuilder"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static EntityTypeBuilder AddQueryFilter<TEntity>(
        this EntityTypeBuilder entityTypeBuilder,
        Expression<Func<TEntity, bool>> expression
    )
        where TEntity : class
    {
        _ = entityTypeBuilder ?? throw new ArgumentNullException(nameof(entityTypeBuilder));
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        var parameter = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
        var newExpression = ReplacingExpressionVisitor.Replace(
            expression.Parameters.Single(),
            parameter,
            expression.Body
        );

        var prevFilter = entityTypeBuilder.Metadata.GetQueryFilter();

        Expression combinedExpression;
        if (prevFilter is null)
        {
            combinedExpression = newExpression;
        }
        else
        {
            var prevExpression = ReplacingExpressionVisitor.Replace(
                prevFilter.Parameters.Single(),
                parameter,
                prevFilter.Body
            );
            combinedExpression = Expression.AndAlso(prevExpression, newExpression);
        }

        return entityTypeBuilder.HasQueryFilter(Expression.Lambda(combinedExpression, parameter));
    }
}
