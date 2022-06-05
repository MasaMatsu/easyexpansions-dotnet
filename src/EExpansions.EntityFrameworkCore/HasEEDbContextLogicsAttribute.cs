using System;

namespace EExpansions.EntityFrameworkCore;

/// <summary>
/// If <see cref="DbContext"/> has this attribute,
/// <see cref="EESaveChangesInterceptor{TContext}"/> skips same hook logics.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class HasEEDbContextLogicsAttribute : Attribute
{ }
