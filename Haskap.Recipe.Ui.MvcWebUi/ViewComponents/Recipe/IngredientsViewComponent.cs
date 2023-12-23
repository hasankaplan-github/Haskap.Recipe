using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class IngredientsViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public IngredientsViewComponent(
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto.Ingredients);
    }
}
