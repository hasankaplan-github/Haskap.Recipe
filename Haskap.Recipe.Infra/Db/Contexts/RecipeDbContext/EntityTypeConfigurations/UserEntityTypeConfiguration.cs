using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Domain.Shared.Consts;

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FirstName)
            .HasMaxLength(UserConsts.MaxFirstNameLength);

        builder.Property(x => x.LastName)
            .HasMaxLength(UserConsts.MaxLastNameLength);

        builder.OwnsOne(a => a.Credentials, x =>
        {
            x.OwnsOne(y => y.Password, y =>
            {
                y.OwnsOne(z => z.Salt);
                y.Ignore(z => z.ClearValue);
            });
            x.HasIndex(y => y.UserName);
        });

        builder.Property(x => x.SystemTimeZoneId)
            .HasMaxLength(UserConsts.MaxSystemTimeZoneIdLength);

        builder.OwnsMany(x => x.Permissions, x =>
        {
            x.WithOwner().HasForeignKey("UserId");
            x.HasKey(x => x.Id);
        });

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
