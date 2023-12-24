using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class UpdateStepModalContentViewComponent : ViewComponent
{
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;
    private readonly IAccountService _accountService;
    private readonly IMultiUserGlobalQueryFilterProvider _multiUserFilter;

    public UpdateStepModalContentViewComponent(
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions,
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter,
        IAccountService accountService,
        IMultiUserGlobalQueryFilterProvider multiUserFilter)
    {
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
        _accountService = accountService;
        _multiUserFilter = multiUserFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, Guid stepId, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        var recipe = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        var step = recipe.Steps
            .Where(x => x.Id == stepId)
            .First();

        return View(step);
    }
}
