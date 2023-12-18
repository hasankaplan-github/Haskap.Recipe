using Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
using Haskap.DddBase.Domain.AuditHistoryLogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class ViewLevelExceptionEntityTypeConfiguration : BaseEntityTypeConfiguration<ViewLevelException>
{
    public override void Configure(EntityTypeBuilder<ViewLevelException> builder)
    {
        base.Configure(builder);
    }
}
