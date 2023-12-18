using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class RecipeCategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<RecipeCategory>
{
    public override void Configure(EntityTypeBuilder<RecipeCategory> builder)
    {
        base.Configure(builder);

        builder.HasOne<Recipe.Domain.RecipeAggregate.Recipe>()
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.RecipeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.RecipeId, x.CategoryId });
    }
}
