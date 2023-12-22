using Haskap.Recipe.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class IngredientsViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;

    public IngredientsViewComponent(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto.Ingredients);
    }
}
