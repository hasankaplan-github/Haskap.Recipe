using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Shared.Consts;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class RecipeEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.RecipeAggregate.Recipe>
{
    public override void Configure(EntityTypeBuilder<Domain.RecipeAggregate.Recipe> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.Name)
            .HasMaxLength(RecipeConsts.MaxNameLength)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(RecipeConsts.MaxDescriptionLength);

        builder.HasMany(x => x.Ingredients)
            .WithOne()
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Steps)
            .WithOne()
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.Picture, x =>
        {
            x.Ignore(y => y.Id);
        });

        builder.OwnsOne(x => x.Slug, x =>
        {
            x.HasIndex(y => y.Value).IsUnique();
        });

        builder.Property(x => x.ViewCount)
            .HasDefaultValue(0);

        builder.HasIndex(x => x.ViewCount);
    }
}
