using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EExpansions.EntityFrameworkCore;

using Internal;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Performs configuration of entity types that inherits a given entity type in the model.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <param name="buildAction">
    /// An action that performs configuration of the entity type that inherits the entity type.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> or <paramref name="buildAction"/> are <see langword="null" />.
    /// </exception>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    public static ModelBuilder Entities<TEntity>(
        this ModelBuilder modelBuilder,
        Action<EntityTypeBuilder> buildAction
    )
        where TEntity : class
    {
        _ = modelBuilder ?? throw new ArgumentNullException();
        _ = buildAction ?? throw new ArgumentNullException();

        var targetTypes =
            modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(TEntity).IsAssignableFrom(t.ClrType));
        foreach (var t in targetTypes)
        {
            buildAction(modelBuilder.Entity(t.ClrType));
        }
        return modelBuilder;
    }

    internal static bool IsEEDbContext<TContext>() where TContext : DbContext =>
        typeof(TContext).GetCustomAttribute<HasEEDbContextLogicsAttribute>(true) is not null;

    /// <summary>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use <see cref="EESaveChangesInterceptor{TContext}"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> is <see langword="null" />.
    /// </exception>
    public static ModelBuilder UseEESaveChangesInterceptor<TContext>(this ModelBuilder modelBuilder)
        where TContext : DbContext
    {
        _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

        if (!IsEEDbContext<TContext>())
        {
            EEDbContextImplementations.OnModelCreating(modelBuilder);
        }

        return modelBuilder;
    }

    #region for struct

    /// <summary>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use <see cref="EESaveChangesInterceptor{TContext, TKey}"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> is <see langword="null" />.
    /// </exception>
    public static ModelBuilder UseEESaveChangesInterceptor<TContext, TKey>(this ModelBuilder modelBuilder)
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
    {
        _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

        if (!IsEEDbContext<TContext>())
        {
            EEDbContextImplementations.OnModelCreating(modelBuilder);
            EEDbContextImplementations.OnModelCreating<TKey>(modelBuilder);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use <see cref="EESaveChangesInterceptor{TContext, TKey}"/>.
    /// <para>
    /// This method performs configuration to activates <see cref="IEntityCreationRecordable{TKey, TUser}"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">The type of the key that is used for user ID.</typeparam>
    /// <typeparam name="TUser">The type of the user entity.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> is <see langword="null" />.
    /// </exception>
    public static ModelBuilder UseEESaveChangesInterceptor<TContext, TKey, TUser>(this ModelBuilder modelBuilder)
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
        where TUser : class
    {
        _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

        if (!IsEEDbContext<TContext>())
        {
            EEDbContextImplementations.OnModelCreating(modelBuilder);
            EEDbContextImplementations.OnModelCreating<TKey>(modelBuilder);
            EEDbContextImplementations.OnModelCreating<TKey, TUser>(modelBuilder);
        }

        return modelBuilder;
    }

    #endregion

    #region for string

    /// <summary>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use <see cref="EESaveChangesInterceptorWithStringKey{TContext}"/>.
    /// </summary>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> is <see langword="null" />.
    /// </exception>
    public static ModelBuilder UseEESaveChangesInterceptorWithStringKey<TContext>(this ModelBuilder modelBuilder)
        where TContext : DbContext
    {
        _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

        if (!IsEEDbContext<TContext>())
        {
            EEDbContextImplementations.OnModelCreating(modelBuilder);
            EEDbContextImplementations.OnModelCreatingWithStringKey(modelBuilder);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Call this method in <see cref="DbContext.OnModelCreating(ModelBuilder)"/> to use <see cref="EESaveChangesInterceptorWithStringKey{TContext}"/>.
    /// </summary>
    /// <typeparam name="TUser">The type of the user entity.</typeparam>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    /// <returns>
    /// The same <see cref="ModelBuilder" /> instance so that additional configuration calls can be chained.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="modelBuilder"/> is <see langword="null" />.
    /// </exception>
    public static ModelBuilder UseEESaveChangesInterceptorWithStringKey<TContext, TUser>(this ModelBuilder modelBuilder)
        where TContext : DbContext
        where TUser : class
    {
        _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

        if (!IsEEDbContext<TContext>())
        {
            EEDbContextImplementations.OnModelCreating(modelBuilder);
            EEDbContextImplementations.OnModelCreatingWithStringKey(modelBuilder);
            EEDbContextImplementations.OnModelCreatingWithStringKey<TUser>(modelBuilder);
        }

        return modelBuilder;
    }

    #endregion
}
