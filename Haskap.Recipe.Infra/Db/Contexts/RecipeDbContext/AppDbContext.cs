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

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IRecipeDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("RecipeDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<Domain.RecipeAggregate.Recipe> Recipe { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Unit> Unit { get; set; }
    public DbSet<IngredientGroup> IngredientGroup { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserRole> UserRole { get; set; }



    public DbSet<ViewLevelException> ViewLevelException { get; set; }
    
}
