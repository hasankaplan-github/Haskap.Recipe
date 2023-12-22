using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class StepsViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly StepPicturesSettings _stepPicturesSettings;

    public StepsViewComponent(
        IRecipeService recipeService,
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions)
    {
        _recipeService = recipeService;
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto.Steps);
    }
}
