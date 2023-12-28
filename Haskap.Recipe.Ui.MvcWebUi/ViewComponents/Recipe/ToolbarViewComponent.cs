using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.UseCaseServices.Accounts;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class ToolbarViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;
    private readonly IAccountService _accountService;
    private readonly IMultiUserGlobalQueryFilterProvider _multiUserFilter;

    public ToolbarViewComponent(
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter,
        IAccountService accountService,
        IMultiUserGlobalQueryFilterProvider multiUserFilter)
    {
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
        _accountService = accountService;
        _multiUserFilter = multiUserFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        var recipeOutputDto = await _recipeService.GetByIdForToolbarViewComponentAsync(recipeId, cancellationToken);

        return View(recipeOutputDto);
    }
}
