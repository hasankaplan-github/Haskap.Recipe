using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class UpdateIngredientModalContentViewComponent : ViewComponent
{
    private readonly IUnitService _unitService;
    private readonly IIngredientGroupService _ingredientGroupService;
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public UpdateIngredientModalContentViewComponent(
        IUnitService unitService,
        IIngredientGroupService ingredientGroupService,
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _unitService = unitService;
        _ingredientGroupService = ingredientGroupService;
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

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
