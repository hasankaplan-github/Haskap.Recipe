using Haskap.Recipe.Application.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Shared;

public class CategoriesViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategoriesViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        return View(categories);

    }
}
