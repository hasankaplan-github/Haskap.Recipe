using Haskap.DddBase.Utilities;
using Haskap.Recipe.Domain.Providers;

namespace Haskap.Recipe.Infra.Providers;
public class MultiUserGlobalQueryFilterProvider : IMultiUserGlobalQueryFilterProvider
{
    public bool IsEnabled { get; private set; } = true;

    public MultiUserGlobalQueryFilterProvider()
    {
    }


    public IDisposable Disable()
    {
        var temp = IsEnabled;
        IsEnabled = false;
        return new DisposeAction(() =>
        {
            IsEnabled = temp;
        });
    }

    public IDisposable Enable()
    {
        var temp = IsEnabled;
        IsEnabled = true;
        return new DisposeAction(() =>
        {
            IsEnabled = temp;
        });
    }
}
