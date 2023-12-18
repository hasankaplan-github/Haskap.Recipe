using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.Common;
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

public class StepEntityTypeConfiguration : BaseEntityTypeConfiguration<Step>
{
    public override void Configure(EntityTypeBuilder<Step> builder)
    {
        base.Configure(builder);

        builder.Property(a => a.Instruction)
            .HasMaxLength(StepConsts.MaxInstructionLength)
            .IsRequired();

        builder.OwnsMany(x => x.Pictures, x =>
        {
            x.HasKey(y => y.Id);
            x.WithOwner().HasForeignKey("StepId");

            x.OwnsOne(y => y.Picture, y =>
            {
                y.Ignore(z => z.Id);
            });
        });
    }
}
