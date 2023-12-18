using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;
public class RecipeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IRecipeService _recipeService;

    public RecipeController(
        ICategoryService categoryService,
        IRecipeService recipeService)
    {
        _categoryService = categoryService;
        _recipeService = recipeService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = (await _categoryService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            .ToList();
        
        return View();
    }

    [HttpPost]
    public async Task<CreateAsDraftOutputDto> CreateAsDraft(CreateAsDraftInputDto inputDto, CancellationToken cancellationToken = default)
    {
        return await _recipeService.CreateAsDraftAsync(inputDto, cancellationToken);
    }

    public async Task<IActionResult> Edit(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = (await _categoryService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            .ToList();

        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto);
    }

    [HttpPut]
    public async Task Activate(Guid id, CancellationToken cancellationToken = default)
    {
        await _recipeService.ActivateAsync(id, cancellationToken);
    }

    [HttpPut]
    public async Task Update(Guid id, UpdateInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _recipeService.UpdateAsync(id, inputDto, cancellationToken);
    }
}
