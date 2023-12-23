using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Haskap.Recipe.Infra.Db.Interceptors;

// burada da TUserId nullable olan Guid? olarak verilecek.
public class MultiUserSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public MultiUserSaveChangesInterceptor(ICurrentUserIdProvider currentUserIdProvider)
    {
        _currentUserIdProvider = currentUserIdProvider;
    }


    private void SetUserId(DbContext dbContext)
    {
        if (_currentUserIdProvider?.CurrentUserId is null)
        {
            return;
        }

        var entityEntries = dbContext.ChangeTracker
                                        .Entries()
                                        .Where(x => x.Entity is IHasMultiUser && 
                                                    (x.State == EntityState.Modified || x.State == EntityState.Added))
                                        .ToList();

        foreach (var entityEntry in entityEntries)
        {
            var multiUserEntity = entityEntry.Entity as IHasMultiUser;
            multiUserEntity.OwnerUserId = _currentUserIdProvider.CurrentUserId.Value;
        }
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetUserId(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetUserId(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
