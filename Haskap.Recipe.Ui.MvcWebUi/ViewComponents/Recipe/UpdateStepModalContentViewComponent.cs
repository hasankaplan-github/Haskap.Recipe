using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class UpdateStepModalContentViewComponent : ViewComponent
{
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly IRecipeService _recipeService;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public UpdateStepModalContentViewComponent(
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions,
        IRecipeService recipeService,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
        _recipeService = recipeService;
        _isDraftFilter = isDraftFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, Guid stepId, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _isDraftFilter.Disable();

        var recipe = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        var step = recipe.Steps
            .Where(x => x.Id == stepId)
            .First();

        return View(step);
    }
}
