using Haskap.DddBase.Domain;
using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.IngredientGroupAggregate;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.RoleAggregate;
using Haskap.Recipe.Domain.UnitAggregate;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain;
public interface IRecipeDbContext : IUnitOfWork
{
    DbSet<RecipeAggregate.Recipe> Recipe { get; set; }
    DbSet<Role> Role { get; set; }
    DbSet<Category> Category { get; set; }
    DbSet<RecipeCategory> RecipeCategory { get; set; }
    DbSet<Unit> Unit { get; set; }
    DbSet<IngredientGroup> IngredientGroup { get; set; }
    DbSet<User> User { get; set; }
    DbSet<UserRole> UserRole { get; set; }




    DbSet<ViewLevelException> ViewLevelException { get; set; }
}
