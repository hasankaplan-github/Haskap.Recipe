using Microsoft.EntityFrameworkCore;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
using Haskap.Recipe.Domain.RoleAggregate;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.UnitAggregate;
using Haskap.Recipe.Domain.IngredientGroupAggregate;
using Haskap.DddBase.Domain;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IRecipeDbContext
{
    private ICurrentUserIdProvider _currentUserIdProvider;
    private Guid? _currentUserId => _currentUserIdProvider?.CurrentUserId;

    private bool _isDraftFilterIsEnabled => GlobalQueryFilterGenericProvider.IsEnabled<IIsDraft>();
    private bool _multiUserFilterIsEnabled => GlobalQueryFilterGenericProvider?.IsEnabled<IHasMultiUser>() ?? false;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        ICurrentUserIdProvider currentUserIdProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("RecipeDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<Domain.RecipeAggregate.Recipe> Recipe { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<RecipeCategory> RecipeCategory { get; set; }
    public DbSet<Unit> Unit { get; set; }
    public DbSet<IngredientGroup> IngredientGroup { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<ViewLevelException> ViewLevelException { get; set; }


    protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType mutableEntityType)
    {
        if (typeof(IIsDraft).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IHasMultiUser).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return base.ShouldFilterEntity<TEntity>(mutableEntityType);
    }

    protected override Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
    {
        var expression = base.CreateFilterExpression<TEntity>();

        if (typeof(IIsDraft).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> isDraftFilterExpression = e => !_isDraftFilterIsEnabled || (e as IIsDraft).IsDraft == false; // EF.Property<bool>(e, "IsActive");
            expression = expression == null ? isDraftFilterExpression : CombineExpressions(expression, isDraftFilterExpression);
        }

        if (typeof(IHasMultiUser).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>>? multiUserExpression = x => !_multiUserFilterIsEnabled || (x as IHasMultiUser).OwnerUserId == _currentUserId;
            expression = expression == null ? multiUserExpression : CombineExpressions(expression, multiUserExpression);
        }

        return expression;
    }

}
