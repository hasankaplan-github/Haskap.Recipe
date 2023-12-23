using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class StepsViewComponent : ViewComponent
{
    private readonly IRecipeService _recipeService;
    private readonly StepPicturesSettings _stepPicturesSettings;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public StepsViewComponent(
        IRecipeService recipeService,
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _recipeService = recipeService;
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
        _isDraftFilter = isDraftFilter;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _isDraftFilter.Disable();

        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto.Steps);
    }
}
