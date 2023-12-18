using Haskap.DddBase.Domain.AuditHistoryLogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class AuditHistoryLogEntityTypeConfiguration : BaseEntityTypeConfiguration<AuditHistoryLog>
{
    public override void Configure(EntityTypeBuilder<AuditHistoryLog> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ModificationType).HasConversion<string>();
        builder.Property(x => x.OwnershipType).HasConversion<string>();
    }
}
