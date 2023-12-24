using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Application.UseCaseServices.Accounts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class UpdateIngredientModalContentViewComponent : ViewComponent
{
    private readonly IUnitService _unitService;
    private readonly IIngredientGroupService _ingredientGroupService;
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;
    private readonly IAccountService _accountService;
    private readonly IMultiUserGlobalQueryFilterProvider _multiUserFilter;

    public UpdateIngredientModalContentViewComponent(
        IUnitService unitService,
        IIngredientGroupService ingredientGroupService,
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter,
        IAccountService accountService,
        IMultiUserGlobalQueryFilterProvider multiUserFilter)
    {
        _unitService = unitService;
        _ingredientGroupService = ingredientGroupService;
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
        _accountService = accountService;
        _multiUserFilter = multiUserFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        var recipe = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        var ingredient = recipe.Ingredients
            .Where(x => x.Id == ingredientId)
            .First();

        ViewBag.RecipeId = recipeId;

        ViewBag.Units = (await _unitService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = ingredient.Amount.UnitId == x.Id
            })
            .Prepend(new SelectListItem
            {
                Value = Guid.Empty.ToString(),
                Text = "<Yeni Ekle>"
            });

        ViewBag.IngredientGroups = (await _ingredientGroupService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = ingredient.IngredientGroupId == x.Id
            })
            .Prepend(new SelectListItem
            {
                Value = Guid.Empty.ToString(),
                Text = "<Yeni Ekle>"
            });

        return View(ingredient);
    }
}
