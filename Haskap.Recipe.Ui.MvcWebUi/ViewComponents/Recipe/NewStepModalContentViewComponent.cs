using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class NewStepModalContentViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;

    public NewStepModalContentViewComponent(
        IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.RecipeId = recipeId;

        ViewBag.StepCount = await _recipeService.GetStepCountAsync(recipeId, cancellationToken);

        return View();
    }
}
