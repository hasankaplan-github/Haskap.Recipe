using Haskap.DddBase.Utilities;
using Haskap.Recipe.Domain.Providers;

namespace Haskap.Recipe.Infra.Providers;
public class IsDraftGlobalQueryFilterProvider : IIsDraftGlobalQueryFilterProvider
{
    public bool IsEnabled { get; private set; } = true;

    private IDisposable ChangeIsDraftFilterStatus(bool isEnabled)
    {
        var oldStatus = IsEnabled;
        IsEnabled = isEnabled;
        return new DisposeAction(() =>
        {
            IsEnabled = oldStatus;
        });
    }

    public IDisposable Disable()
    {
        return ChangeIsDraftFilterStatus(false);
    }

    public IDisposable Enable()
    {
        return ChangeIsDraftFilterStatus(true);
    }
}
