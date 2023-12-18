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

public class IngredientEntityTypeConfiguration : BaseEntityTypeConfiguration<Ingredient>
{
    public override void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(IngredientConsts.MaxNameLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(IngredientConsts.MaxDescriptionLength);

        builder.OwnsOne(x => x.Amount, amountBuilder =>
        {
            amountBuilder.HasOne(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.HasOne(x => x.IngredientGroup)
            .WithMany()
            .HasForeignKey(x => x.IngredientGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
