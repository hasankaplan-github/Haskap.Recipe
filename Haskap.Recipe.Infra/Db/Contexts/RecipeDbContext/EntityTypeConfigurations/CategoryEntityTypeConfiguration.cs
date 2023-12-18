using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class CategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(CategoryConsts.MaxNameLength)
            .IsRequired();

        builder.HasMany(x => x.ChildCategories)
            .WithOne()
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
