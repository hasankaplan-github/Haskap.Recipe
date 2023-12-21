using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Recipe;

public class NewIngredientModalContentViewComponent : ViewComponent
{
    private readonly IUnitService _unitService;
    private readonly IIngredientGroupService _ingredientGroupService;

    public NewIngredientModalContentViewComponent(
        IUnitService unitService,
        IIngredientGroupService ingredientGroupService)
    {
        _unitService = unitService;
        _ingredientGroupService = ingredientGroupService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.RecipeId = recipeId;

        ViewBag.Units = (await _unitService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
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
            })
            .Prepend(new SelectListItem
            {
                Value = Guid.Empty.ToString(),
                Text = "<Yeni Ekle>"
            });

        return View();
    }
}
