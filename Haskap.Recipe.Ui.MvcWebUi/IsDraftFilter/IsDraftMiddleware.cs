using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Ui.MvcWebUi.IsDraftFilter;
public class IsDraftMiddleware
{
    private readonly RequestDelegate _next;

    public IsDraftMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IIsDraftGlobalQueryFilterProvider isDraftGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<IIsDraft>(isDraftGlobalQueryFilterProvider);

        await _next(httpContext);
    }
}
