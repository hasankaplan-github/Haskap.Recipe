using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class NewStepModalContentViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public NewStepModalContentViewComponent(
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.RecipeId = recipeId;

        using var _ = _isDraftFilter.Disable();

        ViewBag.StepCount = await _recipeService.GetStepCountAsync(recipeId, cancellationToken);

        return View();
    }
}
