using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.UseCaseServices.Accounts;
using Haskap.Recipe.Application.UseCaseServices.Categories;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class BasicInfoViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;
    private readonly IAccountService _accountService;
    private readonly IMultiUserGlobalQueryFilterProvider _multiUserFilter;
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly ICategoryService _categoryService;

    public BasicInfoViewComponent(
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter,
        IAccountService accountService,
        IMultiUserGlobalQueryFilterProvider multiUserFilter,
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions,
        ICategoryService categoryService)
    {
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
        _accountService = accountService;
        _multiUserFilter = multiUserFilter;
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        ViewBag.Categories = (await _categoryService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            .ToList();

        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto);

    }
}
