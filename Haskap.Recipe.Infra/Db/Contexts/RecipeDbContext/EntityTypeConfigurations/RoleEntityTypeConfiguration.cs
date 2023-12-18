using Haskap.Recipe.Domain.RoleAggregate;
using Haskap.Recipe.Domain.Shared.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class RoleEntityTypeConfiguration : BaseEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(RoleConsts.MaxNameLength);

        builder.OwnsMany(x => x.Permissions, x =>
        {
            x.WithOwner().HasForeignKey("RoleId");
            x.HasKey(x => x.Id);
        });
    }
}
