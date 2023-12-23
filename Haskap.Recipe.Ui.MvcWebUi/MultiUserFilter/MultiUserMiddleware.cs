using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Ui.MvcWebUi.MultiUserFilter;
public class MultiUserMiddleware
{
    private readonly RequestDelegate _next;

    public MultiUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext, 
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IMultiUserGlobalQueryFilterProvider multiUserGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<IHasMultiUser>(multiUserGlobalQueryFilterProvider);

        await _next(httpContext);
    }
}
